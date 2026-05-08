import pytest
from fastapi.testclient import TestClient
from ai_model import MealRecommender
from main import app

# Instanțiem clientul de test pentru Endpoint-ul de Integrare (FastAPI)
client = TestClient(app)

# Instanțiem Recommender-ul pentru Testele Unitare
recommender = MealRecommender()

# ==========================================
# 1. TESTE UNITARE (Python Pytest)
# ==========================================

def test_categorize_meal_breakfast():
    """Testăm dacă AI-ul categorizează corect o masă ca fiind mic dejun"""
    result = recommender.categorize_meal("healthy breakfast, eggs, toast")
    assert result == "breakfast"

def test_categorize_meal_snack():
    """Testăm dacă AI-ul categorizează corect un snack/desert"""
    result = recommender.categorize_meal("sweet, dessert, cookie")
    assert result == "snack"

def test_categorize_meal_exclude():
    """Testăm dacă exclude băuturile, cocktail-urile, etc."""
    result = recommender.categorize_meal("beverage, cocktail, alcohol")
    assert result == "exclude"

def test_recommend_plan_logic():
    """Test Unitar pentru a verifica dacă logica returnează un plan valid"""
    plan = recommender.recommend_plan(
        daily_calories=2000,
        allergies=[],
        diet_preferences=["BALANCED"],
        days=1,
        goal="Maintain",
        activity_level="Active"
    )
    
    assert type(plan) == list
    assert len(plan) == 1 # Cerut pentru o singură zi
    assert plan[0]["day"] == 1
    
    meals = plan[0]["meals"]
    assert len(meals) == 4 # Breakfast, Lunch, Snack, Dinner

# ==========================================
# 2. TESTE DE INTEGRARE AI (REST Endpoints)
# ==========================================

def test_api_generate_plan_success():
    """Testăm dacă Endpoint-ul API-ului răspunde corect cu codul 200 și returnează un JSON valid"""
    payload = {
        "dailyCalorieTarget": 2500,
        "allergies": ["Peanuts"],
        "dietPreferences": ["VEGAN"],
        "days": 2,
        "goal": "GainWeight",
        "activityLevel": "Moderate"
    }

    response = client.post("/api/ai/generate-plan", json=payload)
    
    assert response.status_code == 200
    data = response.json()
    
    assert "planDays" in data
    assert len(data["planDays"]) == 2 # Am cerut 2 zile
    
    first_day = data["planDays"][0]
    assert len(first_day["meals"]) > 0

def test_api_generate_plan_invalid_days():
    """Testăm validarea Pydantic (zilele trebuie să fie între 1 și 7)"""
    payload = {
        "dailyCalorieTarget": 2500,
        "allergies": [],
        "dietPreferences": [],
        "days": 10, # Invalid, trebuie <= 7
        "goal": "LoseWeight",
        "activityLevel": "Light"
    }

    response = client.post("/api/ai/generate-plan", json=payload)
    
    # Ne așteptăm la o eroare Unprocessable Entity (422) generată de FastAPI / Pydantic
    assert response.status_code == 422