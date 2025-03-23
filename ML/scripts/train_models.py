import pandas as pd
from sklearn.model_selection import train_test_split, cross_val_score
from sklearn.ensemble import RandomForestClassifier
import pickle

# Đọc dataset
df = pd.read_csv("../data/medical_data_from_sql.csv", encoding="utf-8-sig")

# Định nghĩa các cột đặc trưng (chỉ sử dụng các cột triệu chứng)
feature_cols = [col for col in df.columns if col.startswith("Triệu chứng_")]
X = df[feature_cols]

# Định nghĩa nhãn
y_diagnosis = df["Mã chẩn đoán"]
y_treatment = df["Mã phương pháp điều trị"]
medicine_cols = [col for col in df.columns if col.startswith("Thuốc_")]

# Huấn luyện mô hình cho chẩn đoán
model_diagnosis = RandomForestClassifier(
    n_estimators=200, max_depth=15, min_samples_split=3, min_samples_leaf=1, random_state=42
)
X_train_diag, X_test_diag, y_diag_train, y_diag_test = train_test_split(X, y_diagnosis, test_size=0.2, random_state=42)
model_diagnosis.fit(X_train_diag, y_diag_train)
print("Đã huấn luyện mô hình chẩn đoán. Độ chính xác trên tập kiểm tra:", 
      model_diagnosis.score(X_test_diag, y_diag_test))
cv_scores_diag = cross_val_score(model_diagnosis, X, y_diagnosis, cv=5)
print("Độ chính xác trung bình (cross-validation) cho chẩn đoán:", cv_scores_diag.mean())

# Huấn luyện mô hình cho phương pháp điều trị
model_treatment = RandomForestClassifier(
    n_estimators=200, max_depth=15, min_samples_split=3, min_samples_leaf=1, random_state=42
)
X_train_treat, X_test_treat, y_treat_train, y_treat_test = train_test_split(X, y_treatment, test_size=0.2, random_state=42)
model_treatment.fit(X_train_treat, y_treat_train)
print("Đã huấn luyện mô hình phương pháp điều trị. Độ chính xác trên tập kiểm tra:", 
      model_treatment.score(X_test_treat, y_treat_test))
cv_scores_treat = cross_val_score(model_treatment, X, y_treatment, cv=5)
print("Độ chính xác trung bình (cross-validation) cho phương pháp điều trị:", cv_scores_treat.mean())

# Huấn luyện mô hình cho thuốc (multi-label classification)
from sklearn.multioutput import MultiOutputClassifier
model_medicine = MultiOutputClassifier(RandomForestClassifier(
    n_estimators=200, max_depth=15, min_samples_split=3, min_samples_leaf=1, random_state=42
))
y_medicines = df[medicine_cols]
X_train_med, X_test_med, y_med_train, y_med_test = train_test_split(X, y_medicines, test_size=0.2, random_state=42)
model_medicine.fit(X_train_med, y_med_train)
print("Đã huấn luyện mô hình thuốc. Độ chính xác trên tập kiểm tra:", 
      model_medicine.score(X_test_med, y_med_test))
cv_scores_med = cross_val_score(model_medicine, X, y_medicines, cv=5)
print("Độ chính xác trung bình (cross-validation) cho thuốc:", cv_scores_med.mean())

# Lưu các mô hình
with open("../models/diagnosis_model.pkl", "wb") as f:
    pickle.dump(model_diagnosis, f)
with open("../models/treatment_model.pkl", "wb") as f:
    pickle.dump(model_treatment, f)
with open("../models/medicine_model.pkl", "wb") as f:
    pickle.dump(model_medicine, f)

print("Đã lưu các mô hình thành công!")