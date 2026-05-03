# Run Command: uvicorn main:app --reload
import kagglehub
import pandas as pd
import os
import re

class MealRecommender:
    def __init__(self):
        # Download dataset
        dataset_path = kagglehub.dataset_download("shuyangli94/food-com-recipes-and-user-interactions")
        csv_file = os.path.join(dataset_path, "RAW_recipes.csv")

        # We load the dataset and keep only the necessary columns
        self.df = pd.read_csv(csv_file)[['name', 'ingredients', 'tags', 'nutrition']].dropna()
        self.df.rename(columns={'name': 'title'}, inplace=True)

        
        # Nutrition Extraction
        nutrition_df = self.df['nutrition'].str.strip('[]').str.split(',', expand=True).astype(float)
        
        self.df['calories'] = nutrition_df[0]
        self.df['fat'] = nutrition_df[1] * 0.65       
        self.df['protein'] = nutrition_df[4] * 0.50   
        self.df['carbs'] = nutrition_df[6] * 3.00     

        # We clean up caloric aberrations
        self.df = self.df[(self.df['calories'] > 50) & (self.df['calories'] < 2500)]
        
        # word processing
        # Convert to lowercase and remove punctuation for a clean search
        self.df['ing_text'] = self.df['ingredients'].str.lower().str.replace(r'[^a-z\s]', ' ', regex=True)
        self.df.reset_index(drop=True, inplace=True)

        # plural generator
        # A small function that takes the word "berry" and automatically adds "berries", "tomato" -> "tomatoes"
        def get_plurals(words):
            res = set()
            for w in words:
                res.add(w)
                if w.endswith('y'):
                    res.add(w[:-1] + 'ies')
                elif w.endswith('s') or w.endswith('x') or w.endswith('ch') or w.endswith('sh'):
                    res.add(w + 'es')
                else:
                    res.add(w + 's')
            return list(res)

        
        # massive dictionary of ingredients
        MEATS = get_plurals([
            "meat", "beef", "pork", "chicken", "turkey", "lamb", "duck", "veal", 
            "sausage", "bacon", "ham", "prosciutto", "pepperoni", "salami", "chorizo", 
            "meatball", "hamburger", "steak", "rib", "venison", "rabbit", "poultry", 
            "frankfurter", "hotdog", "hot dog", "wiener", "mutton", "brisket", "kielbasa", 
            "pastrami", "pancetta", "boar", "bologna", "bratwurst", "chop", "sirloin", 
            "chuck", "roast", "meatloaf", "tenderloin", "liver", "lard", "gelatin"
        ])
        
        SEAFOOD = get_plurals([
            "fish", "salmon", "tuna", "cod", "trout", "halibut", "swordfish", "mackerel", 
            "shrimp", "prawn", "crab", "lobster", "clam", "oyster", "scallop", "squid", 
            "octopus", "anchovy", "sardine", "mahi", "tilapia", "snapper", "mussel", 
            "crawfish", "calamari", "haddock", "catfish", "flounder", "caviar", "crabmeat"
        ])
        
        DAIRY = get_plurals([
            "milk", "cheese", "butter", "cream", "yogurt", "whey", "casein", "parmesan", 
            "cheddar", "mozzarella", "ricotta", "ghee", "paneer", "brie", "gouda", "kefir", "buttermilk"
        ])
        
        EGGS = get_plurals(["egg", "mayo", "mayonnaise"])
        
        GRAINS_CARBS = get_plurals([
            "wheat", "flour", "bread", "pasta", "noodle", "gluten", "barley", "rye", "oat", 
            "oatmeal", "rice", "corn", "cornmeal", "cornstarch", "potato", "yam", "quinoa", 
            "bun", "roll", "crust", "dough", "pastry", "tortilla", "wrap", "pita", "couscous", 
            "polenta", "macaroni", "spaghetti", "cereal", "bran", "tapioca", "arrowroot", 
            "cracker", "pretzel", "bagel", "muffin", "pancake", "waffle"
        ])
        
        LEGUMES = get_plurals([
            "bean", "lentil", "dal", "dhal", "chickpea", "pea", "peanut", "soy", 
            "tofu", "tempeh", "hummus", "edamame"
        ])
        
        SUGARS = get_plurals([
            "sugar", "honey", "syrup", "maple", "agave", "chocolate", "cocoa", "candy", 
            "marshmallow", "cookie", "cake", "pie", "brownie", "frosting", "caramel", 
            "jam", "jelly", "marmalade", "ketchup"
        ])
        
        VEGETABLES = get_plurals([
            "vegetable", "tomato", "pepper", "spinach", "lettuce", "cabbage", "broccoli", 
            "cauliflower", "carrot", "squash", "zucchini", "cucumber", "celery", "asparagus", 
            "eggplant", "mushroom", "olive", "caper", "leek", "radish", "turnip", "beet", 
            "artichoke", "kale", "pumpkin", "jalapeno", "chili"
        ]) 
        
        FRUITS = get_plurals([
            "fruit", "apple", "banana", "orange", "lemon", "lime", "grape", "berry", 
            "strawberry", "blueberry", "cranberry", "apricot", "peach", "pear", "plum", 
            "pomegranate", "melon", "watermelon", "cantaloupe", "cherry", "mango", 
            "pineapple", "coconut", "avocado", "raisin", "date", "fig"
        ])
        
        NUTS_SEEDS = get_plurals([
            "almond", "walnut", "pecan", "cashew", "pistachio", "hazelnut", "macadamia", 
            "pine nut", "sunflower", "chia", "flax", "sesame", "tahini"
        ])

        
        # allergies & diets (exclusion rules)
        self.allergy_keywords = {
            "Peanuts": get_plurals(["peanut", "groundnut"]),
            "Tree Nuts": NUTS_SEEDS + get_plurals(["nut"]),
            "Milk (Dairy/Lactose)": DAIRY,
            "Eggs": EGGS,
            "Wheat (Gluten)": GRAINS_CARBS, 
            "Soy": get_plurals(["soy", "soya", "tofu", "miso", "tempeh", "edamame"]),
            "Fish": SEAFOOD,
            "Shellfish": get_plurals(["shrimp", "prawn", "crab", "lobster", "clam", "oyster", "scallop", "mussel", "crawfish"]),
            "Sesame": get_plurals(["sesame", "tahini"]),
            "Mustard": get_plurals(["mustard"]),
            "Celery": get_plurals(["celery"]),
            "Sulfites": get_plurals(["wine", "vinegar"]),
            "Lupin": get_plurals(["lupin"])
        }

        self.diet_rules = {
            "VEGAN": {"exclude": MEATS + SEAFOOD + DAIRY + EGGS + get_plurals(["honey", "gelatin"])},
            "VEGETARIAN": {"exclude": MEATS + SEAFOOD + get_plurals(["gelatin"])},
            "PESCATARIAN": {"exclude": MEATS + get_plurals(["gelatin"])},
            "KETO": {"exclude": GRAINS_CARBS + SUGARS + LEGUMES + FRUITS},
            "PALEO": {"exclude": DAIRY + GRAINS_CARBS + LEGUMES + SUGARS},
            "CARNIVORE": {"exclude": GRAINS_CARBS + LEGUMES + SUGARS + VEGETABLES + FRUITS + NUTS_SEEDS},
            "MEDITERRANEAN": {},
            "BALANCED": {}
        }

    
    # meal classification
    def categorize_meal(self, tags_str):
        tags = str(tags_str).lower()
        
        if any(bad_tag in tags for bad_tag in ['beverage', 'condiment', 'cocktail', 'drink', 'syrup', 'frosting']):
            return 'exclude'
            
        if 'breakfast' in tags:
            return 'breakfast'
        elif any(k in tags for k in ['snack', 'dessert', 'appetizer', 'cookie', 'smoothie']):
            return 'snack'
        elif any(k in tags for k in ['lunch', 'sandwich', 'salad']):
            return 'lunch'
        else:
            return 'dinner'

    
    # main function
    def recommend_plan(self, daily_calories, allergies, diet_preferences, days, goal, activity_level):
        df = self.df.copy()
        daily_calories = float(daily_calories)

        safe_days = max(1, min(int(days), 7))

        # filter allergies & diets
        avoid_keywords = set()
        
        for allergy in allergies:
            avoid_keywords.update(self.allergy_keywords.get(allergy, []))

        for diet in diet_preferences:
            rules = self.diet_rules.get(str(diet).upper(), {})
            if "exclude" in rules:
                avoid_keywords.update(rules["exclude"])

        if avoid_keywords:
            # We create a Regex filter exactly based on our massive list of plurals
            # We use \b to find only single words.
            pattern = r'\b(?:' + '|'.join(re.escape(kw) for kw in avoid_keywords) + r')\b'
            mask = df['ing_text'].str.contains(pattern, regex=True, na=False)
            df = df[~mask] 

        if df.empty:
            return [] # In case of incompatible diets that delete the entire dataset

       
        # classify meals
        df['meal_category'] = df['tags'].apply(self.categorize_meal)
        df = df[df['meal_category'] != 'exclude']

        breakfast_df = df[df['meal_category'] == 'breakfast'].copy()
        snack_df = df[df['meal_category'] == 'snack'].copy()
        lunch_df = df[df['meal_category'] == 'lunch'].copy()
        dinner_df = df[df['meal_category'] == 'dinner'].copy()

        if breakfast_df.empty: breakfast_df = df.copy()
        if snack_df.empty: snack_df = df.copy()
        if lunch_df.empty: lunch_df = df.copy()
        if dinner_df.empty: dinner_df = df.copy()

        # duplicate elimitation
        breakfast_df.drop_duplicates(subset=['title'], inplace=True)
        snack_df.drop_duplicates(subset=['title'], inplace=True)
        lunch_df.drop_duplicates(subset=['title'], inplace=True)
        dinner_df.drop_duplicates(subset=['title'], inplace=True)

        # calorie distribution
        cal_targets = {
            "Breakfast": daily_calories * 0.25,
            "Snack": daily_calories * 0.10,
            "Lunch": daily_calories * 0.35,
            "Dinner": daily_calories * 0.30
        }

        protein_pct = 0.35 if goal.lower() == "loseweight" else (0.25 if goal.lower() == "gainweight" else 0.30)

        def score(df, target):
            protein_target = (target * protein_pct) / 4
            return (df['calories'] - target).abs() + (df['protein'] - protein_target).abs() * 2

        breakfast_df['score'] = score(breakfast_df, cal_targets["Breakfast"])
        snack_df['score'] = score(snack_df, cal_targets["Snack"])
        lunch_df['score'] = score(lunch_df, cal_targets["Lunch"])
        dinner_df['score'] = score(dinner_df, cal_targets["Dinner"])

       
        # select top meals
        pool_size = max(safe_days * 4, 10)
        top_b = breakfast_df.nsmallest(pool_size, 'score').sample(frac=1).to_dict('records')
        top_s = snack_df.nsmallest(pool_size, 'score').sample(frac=1).to_dict('records')
        top_l = lunch_df.nsmallest(pool_size, 'score').sample(frac=1).to_dict('records')
        top_d = dinner_df.nsmallest(pool_size, 'score').sample(frac=1).to_dict('records')

        
        # build plan
        plan = []

        for i in range(safe_days):
            def pick(lst, idx):
                return lst[idx % len(lst)] if lst else None

            meals = []
            
            for mtype, lst in [("Breakfast", top_b), ("Lunch", top_l), ("Snack", top_s), ("Dinner", top_d)]:
                row = pick(lst, i)
                if not row: continue

                clean_ingredients = str(row['ingredients']).replace('[', '').replace(']', '').replace("'", "")

                meals.append({
                    "mealType": mtype,
                    "name": str(row['title']).title(),
                    "description": "Ingredients: " + clean_ingredients[:400] + ("..." if len(clean_ingredients) > 400 else ""),
                    "calories": round(float(row['calories'])),
                    "protein": round(float(row['protein']), 1),
                    "fat": round(float(row['fat']), 1),
                    "carbs": round(float(row['carbs']), 1)
                })

            plan.append({
                "day": i + 1,
                "meals": meals
            })

        return plan