import pandas as pd
from sklearn.model_selection import train_test_split, cross_val_score
from sklearn.ensemble import RandomForestClassifier
import pickle

# Đọc dataset
df = pd.read_csv("../data/medical_data_from_sql.csv", encoding="utf-8-sig")

# Định nghĩa các cột đặc trưng (chỉ sử dụng các cột triệu chứng)
feature_cols = [col for col in df.columns if col.startswith("Triệu chứng_")]
X = df[feature_cols]

# Định nghĩa các nhãn (labels)
y_diagnosis = df["Mã chẩn đoán"]
y_treatment = df["Mã phương pháp điều trị"]

# Chia dữ liệu thành tập huấn luyện và kiểm tra
X_train_diag, X_test_diag, y_diag_train, y_diag_test = train_test_split(
    X, y_diagnosis, test_size=0.2, random_state=42
)
X_train_treat, X_test_treat, y_treat_train, y_treat_test = train_test_split(
    X, y_treatment, test_size=0.2, random_state=42
)

# Huấn luyện mô hình cho chẩn đoán
model_diagnosis = RandomForestClassifier(
    n_estimators=100,
    max_depth=10,  # Giới hạn độ sâu của cây
    min_samples_split=5,  # Số mẫu tối thiểu để chia một nút
    min_samples_leaf=2,  # Số mẫu tối thiểu ở mỗi lá
    random_state=42
)
model_diagnosis.fit(X_train_diag, y_diag_train)
print("Đã huấn luyện mô hình chẩn đoán. Độ chính xác trên tập kiểm tra:", 
      model_diagnosis.score(X_test_diag, y_diag_test))

# Thêm cross-validation để kiểm tra overfitting
cv_scores_diag = cross_val_score(model_diagnosis, X, y_diagnosis, cv=5)
print("Độ chính xác trung bình (cross-validation) cho chẩn đoán:", cv_scores_diag.mean())

# Huấn luyện mô hình cho phương pháp điều trị
model_treatment = RandomForestClassifier(
    n_estimators=100,
    max_depth=10,
    min_samples_split=5,
    min_samples_leaf=2,
    random_state=42
)
model_treatment.fit(X_train_treat, y_treat_train)
print("Đã huấn luyện mô hình phương pháp điều trị. Độ chính xác trên tập kiểm tra:", 
      model_treatment.score(X_test_treat, y_treat_test))

# Thêm cross-validation để kiểm tra overfitting
cv_scores_treat = cross_val_score(model_treatment, X, y_treatment, cv=5)
print("Độ chính xác trung bình (cross-validation) cho phương pháp điều trị:", cv_scores_treat.mean())

# Lưu các mô hình
with open("../models/diagnosis_model.pkl", "wb") as f:
    pickle.dump(model_diagnosis, f)
with open("../models/treatment_model.pkl", "wb") as f:
    pickle.dump(model_treatment, f)

print("Đã lưu các mô hình thành công!")