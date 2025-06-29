#!/usr/bin/env python
# coding: utf-8


from flask import Flask, request, jsonify
import numpy as np
import joblib

app = Flask(__name__)

# تحميل الموديل والسكيلر واسماء الأعمدة المتوقعة
model = joblib.load('best_model.pkl')
scaler = joblib.load('scaler.pkl')
EXPECTED_FEATURES = joblib.load('expected_features.pkl')

# قيم افتراضية للأعمدة التي لن تأتي من المستخدم
DEFAULTS = {
    'LDLC': 120,
    'HDLC': 50,
    'PREVCHD': 0,
    'PREVAP': 0,
    'PREVMI': 0,
    'PREVSTRK': 0,
    'PREVHYP': 0,
    'TIME': 0,
    'PERIOD': 5,
    'ANYCHD': 0,
    'CVD': 0,
    'HYPERTEN': 0,
    # أضف باقي الأعمدة الافتراضية هنا حسب الحاجة
}

def calculate_bmi(weight_kg, height_cm):
    if height_cm == 0:
        return 0
    height_m = height_cm / 100
    return round(weight_kg / (height_m ** 2), 2)

def analyze_metrics(sysbp, diabp, bmi, heartrate, glucose, cholesterol):
    # تحليل الضغط الانقباضي (SYSBP)
    if sysbp >= 140:
        sysbp_status = "Stage 2 Hypertension"
        sysbp_recommendation = "Visit your doctor immediately"
    elif sysbp >= 130:
        sysbp_status = "Stage 1 Hypertension"
        sysbp_recommendation = "Consult your doctor soon"
    elif sysbp >= 120:
        sysbp_status = "Elevated"
        sysbp_recommendation = "Maintain healthy lifestyle"
    else:
        sysbp_status = "Normal"
        sysbp_recommendation = "Maintain it"

    # تحليل الضغط الانبساطي (DIABP)
    if diabp >= 90:
        diabp_status = "Stage 2 Hypertension"
        diabp_recommendation = "Visit your doctor immediately"
    elif diabp >= 80:
        diabp_status = "Stage 1 Hypertension"
        diabp_recommendation = "Consult your doctor soon"
    else:
        diabp_status = "Normal"
        diabp_recommendation = "Maintain it"

    # تحليل BMI
    if bmi >= 30:
        bmi_status = "Obese"
        bmi_recommendation = "Consider weight loss"
    elif bmi >= 25:
        bmi_status = "Overweight"
        bmi_recommendation = "Consider weight loss"
    elif bmi > 18.5:
        bmi_status = "Normal"
        bmi_recommendation = "Maintain it"
    else:
        bmi_status = "Underweight"
        bmi_recommendation = "Consider gaining weight"

    # تحليل Heart Rate
    if heartrate < 60:
        hr_status = "Below Normal"
        hr_recommendation = "Consult your doctor"
    elif heartrate <= 100:
        hr_status = "Normal"
        hr_recommendation = "Maintain it"
    else:
        hr_status = "Above Normal"
        hr_recommendation = "Consult your doctor"

    # تحليل Glucose
    if glucose >= 126:
        glucose_status = "Diabetes"
        glucose_recommendation = "Medical advice needed"
    elif glucose >= 100:
        glucose_status = "Prediabetes"
        glucose_recommendation = "Watch your sugar intake"
    else:
        glucose_status = "Normal"
        glucose_recommendation = "Maintain healthy diet"

    # تحليل Cholesterol
    if cholesterol >= 240:
        chol_status = "High"
        chol_recommendation = "Medical advice needed"
    elif cholesterol >= 200:
        chol_status = "Borderline High"
        chol_recommendation = "Consider diet changes"
    else:
        chol_status = "Desirable"
        chol_recommendation = "Maintain healthy diet"

    return {
        "Systolic BP": {"Value": sysbp, "Status": sysbp_status, "Recommendation": sysbp_recommendation},
        "Diastolic BP": {"Value": diabp, "Status": diabp_status, "Recommendation": diabp_recommendation},
        "BMI": {"Value": bmi, "Status": bmi_status, "Recommendation": bmi_recommendation},
        "Heart Rate": {"Value": heartrate, "Status": hr_status, "Recommendation": hr_recommendation},
        "Glucose": {"Value": glucose, "Status": glucose_status, "Recommendation": glucose_recommendation},
        "Cholesterol": {"Value": cholesterol, "Status": chol_status, "Recommendation": chol_recommendation},
    }

def build_full_feature_vector(user_input):
    vector = {}

    # تحويل بيانات المستخدم
    vector['SEX'] = 1 if user_input.get("gender", "").lower() == "male" else 2
    vector['AGE'] = user_input.get("age", 0)
    vector['CURSMOKE'] = 1 if user_input.get("smokes", False) else 0
    vector['CIGPDAY'] = user_input.get("cigarettes_per_day", 0)
    vector['BMI'] = calculate_bmi(user_input.get("weight_kg", 0), user_input.get("height_cm", 0))
    vector['DIABETES'] = 1 if user_input.get("diabetes", False) else 0
    vector['BPMEDS'] = 1 if user_input.get("bp_meds", False) else 0
    vector['HEARTRTE'] = user_input.get("heart_rate", 0)
    vector['GLUCOSE'] = user_input.get("glucose", 0)
    vector['educ'] = {
        "primary": 1,
        "secondary": 2,
        "college": 3,
        "graduate": 4
    }.get(user_input.get("education", "").lower(), 1)
    vector['TOTCHOL'] = user_input.get("cholesterol", 200)

    # ملء باقي القيم الافتراضية
    for feat in EXPECTED_FEATURES:
        if feat not in vector:
            vector[feat] = DEFAULTS.get(feat, 0)

    # ترتيب المتجه حسب ترتيب الأعمدة
    ordered_vector = [vector[feat] for feat in EXPECTED_FEATURES]
    return np.array([ordered_vector]), vector

@app.route('/predict', methods=['POST'])
def predict():
    try:
        data = request.json
        features_np, feature_dict = build_full_feature_vector(data)
        scaled_features = scaler.transform(features_np)
        prediction = model.predict(scaled_features)

        sysbp_pred = float(prediction[0][0])
        diabp_pred = float(prediction[0][1])
        bmi_val = feature_dict['BMI']
        hr_val = feature_dict['HEARTRTE']
        glucose_val = feature_dict['GLUCOSE']
        chol_val = feature_dict['TOTCHOL']

        analysis = analyze_metrics(sysbp_pred, diabp_pred, bmi_val, hr_val, glucose_val, chol_val)

        return jsonify({
            "predictions": {
                "SYSBP": sysbp_pred,
                "DIABP": diabp_pred
            },
            "metrics_analysis": analysis
        })

    except Exception as e:
        return jsonify({"error": str(e)}), 400

if __name__ == '__main__':
    app.run(debug=False, use_reloader=False)


