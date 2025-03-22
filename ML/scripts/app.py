from flask import Flask, request, jsonify, make_response
from flask_cors import CORS
import pandas as pd
import pickle
import json

app = Flask(__name__)
CORS(app)  # Cho phép CORS để ASP.NET Core có thể gọi API

# Đọc ánh xạ và mô hình
with open("../models/diagnosis_map_vn.pkl", "rb") as f:
    diagnosis_map = pickle.load(f)
with open("../models/treatment_map_vn.pkl", "rb") as f:
    treatment_map = pickle.load(f)
with open("../models/diagnosis_model.pkl", "rb") as f:
    model_diagnosis = pickle.load(f)
with open("../models/treatment_model.pkl", "rb") as f:
    model_treatment = pickle.load(f)

# Đọc dataset để lấy danh sách triệu chứng
df = pd.read_csv("../data/medical_data_from_sql.csv", encoding="utf-8-sig")
symptom_cols = [col for col in df.columns if col.startswith("Triệu chứng_")]
symptoms = [col.replace("Triệu chứng_", "") for col in symptom_cols]

def predict_diagnosis_and_treatment(user_symptoms):
    symptom_vector = [0] * len(symptom_cols)
    for symptom in user_symptoms:
        if symptom in symptoms:
            idx = symptoms.index(symptom)
            symptom_vector[idx] = 1
    
    input_data = pd.DataFrame([symptom_vector], columns=symptom_cols)
    diag_pred = model_diagnosis.predict(input_data)[0]
    diagnosis = diagnosis_map[diag_pred]
    treat_pred = model_treatment.predict(input_data)[0]
    treatment = treatment_map[treat_pred]
    return diagnosis, treatment

@app.route('/api/prediction/symptoms', methods=['GET'])
def get_symptoms():
    # Sử dụng json.dumps để kiểm soát mã hóa
    response_data = json.dumps(symptoms, ensure_ascii=False)
    return make_response(response_data, 200, {'Content-Type': 'application/json; charset=utf-8'})

@app.route('/api/prediction/predict', methods=['POST'])
def predict():
    try:
        data = request.get_json()
        print("Received data:", data)
        user_symptoms = data.get('symptoms', [])
        if not user_symptoms:
            response_data = json.dumps({"error": "Vui lòng cung cấp ít nhất một triệu chứng!"}, ensure_ascii=False)
            return make_response(response_data, 400, {'Content-Type': 'application/json; charset=utf-8'})

        diagnosis, treatment = predict_diagnosis_and_treatment(user_symptoms)
        print("Prediction result:", {"diagnosis": diagnosis, "treatment": treatment})
        response_data = json.dumps({
            "diagnosis": diagnosis,
            "treatment": treatment
        }, ensure_ascii=False)
        return make_response(response_data, 200, {'Content-Type': 'application/json; charset=utf-8'})
    except Exception as e:
        print("Error:", str(e))
        response_data = json.dumps({"error": str(e)}, ensure_ascii=False)
        return make_response(response_data, 500, {'Content-Type': 'application/json; charset=utf-8'})
if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=5000)