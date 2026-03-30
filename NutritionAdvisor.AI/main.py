from fastapi import FastAPI
from pydantic import BaseModel
from typing import List
from ai_model import MealRecommender

app = FastAPI()
recommender = MealRecommender()

class RecommendationRequest(BaseModel):
    dailyCalorieTarget: float
    allergies: List[str] = []
    dietPreferences: List[str] = []
    days: int
    goal: str
    activityLevel: str

@app.post("/api/ai/generate-plan")
def generate_meal_plan(request: RecommendationRequest):
    plan = recommender.recommend_plan(
        daily_calories=request.dailyCalorieTarget,
        allergies=request.allergies,
        diet_preferences=request.dietPreferences,
        days=request.days,
        goal=request.goal,
        activity_level=request.activityLevel
    )
    return {"planDays": plan}