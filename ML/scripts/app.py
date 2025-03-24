from flask import Flask, request, jsonify, make_response
from flask_cors import CORS
import pandas as pd
import pickle
import json

app = Flask(__name__)
CORS(app)

# Đọc ánh xạ và mô hình
with open("../models/diagnosis_map_vn.pkl", "rb") as f:
    diagnosis_map = pickle.load(f)
with open("../models/treatment_map_vn.pkl", "rb") as f:
    treatment_map = pickle.load(f)
with open("../models/medicine_map.pkl", "rb") as f:
    medicine_map = pickle.load(f)
with open("../models/diagnosis_model.pkl", "rb") as f:
    model_diagnosis = pickle.load(f)
with open("../models/treatment_model.pkl", "rb") as f:
    model_treatment = pickle.load(f)
with open("../models/medicine_model.pkl", "rb") as f:
    model_medicine = pickle.load(f)

# Đọc dataset để lấy danh sách triệu chứng và thuốc
df = pd.read_csv("../data/medical_data_from_sql.csv", encoding="utf-8-sig")
symptom_cols = [col for col in df.columns if col.startswith("Triệu chứng_")]
symptoms = [col.replace("Triệu chứng_", "") for col in symptom_cols]
medicine_cols = [col for col in df.columns if col.startswith("Thuốc_")]
medicines = [col.replace("Thuốc_", "") for col in medicine_cols]

def predict_diagnosis_treatment_and_medicines(user_symptoms):
    symptom_vector = [0] * len(symptom_cols)
    for symptom in user_symptoms:
        if symptom in symptoms:
            idx = symptoms.index(symptom)
            symptom_vector[idx] = 1
    
    input_data = pd.DataFrame([symptom_vector], columns=symptom_cols)
    
    # Dự đoán chẩn đoán với xác suất
    diag_probs = model_diagnosis.predict_proba(input_data)[0]
    diag_pred = model_diagnosis.predict(input_data)[0]
    max_prob_diag = max(diag_probs)
    diagnosis = diagnosis_map[diag_pred]
    
    # Dự đoán phương pháp điều trị với xác suất
    treat_probs = model_treatment.predict_proba(input_data)[0]
    treat_pred = model_treatment.predict(input_data)[0]
    max_prob_treat = max(treat_probs)
    treatment = treatment_map[treat_pred]
    
    # Dự đoán thuốc
    med_pred = model_medicine.predict(input_data)[0]
    selected_medicines = [medicines[i] for i, pred in enumerate(med_pred) if pred == 1]
    if not selected_medicines and treatment in medicine_map:
        selected_medicines = [m["name"] for m in medicine_map[treatment]]
    
    medicine_details = {}
    for med in selected_medicines:
        for treatment_med_list in medicine_map.values():
            for m in treatment_med_list:
                if m["name"] == med and med not in medicine_details:
                    medicine_details[med] = m
                    break
    unique_medicines = list(medicine_details.values())
    
    # Kiểm tra độ tin cậy
    if max_prob_diag < 0.5 or max_prob_treat < 0.5:  # Ngưỡng xác suất
        return diagnosis, treatment, unique_medicines, "Cảnh báo: Dự đoán có thể không chính xác do thiếu triệu chứng, thêm nhiều triệu chứng để tăng độ chính xác."
    
    return diagnosis, treatment, unique_medicines, None

@app.route('/api/prediction/symptoms', methods=['GET'])
def get_symptoms():
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
        
        diagnosis, treatment, medicines, warning = predict_diagnosis_treatment_and_medicines(user_symptoms)
        response = {
            "diagnosis": diagnosis,
            "treatment": treatment,
            "medicines": medicines
        }
        if warning:
            response["warning"] = warning
        print("Prediction result:", response)
        response_data = json.dumps(response, ensure_ascii=False)
        return make_response(response_data, 200, {'Content-Type': 'application/json; charset=utf-8'})
    except Exception as e:
        print("Error:", str(e))
        response_data = json.dumps({"error": str(e)}, ensure_ascii=False)
        return make_response(response_data, 500, {'Content-Type': 'application/json; charset=utf-8'})

if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=5000)