import pandas as pd
import numpy as np
import pickle

# Danh sách bệnh và phương pháp điều trị
diseases = [
    "Viêm họng cấp", "Cận thị", "Tăng huyết áp", "Sốt xuất huyết", "Thiếu máu", "Viêm dạ dày",
    "Gãy xương cẳng tay", "Viêm da dị ứng", "Viêm thận cấp", "Sốt virus", "Viêm xoang mạn",
    "Đau lưng cơ học", "Tiểu đường type 2", "Chàm (eczema)", "Suy tim nhẹ", "Viêm phổi",
    "Sỏi thận", "Viêm gan B", "Đau nửa đầu (migraine)", "Viêm khớp dạng thấp", "Hen suyễn",
    "Loét dạ dày", "Nhiễm trùng đường tiểu (UTI)", "Viêm ruột thừa", "Đột quỵ"
]

treatments = [
    "Uống kháng sinh và giảm đau", "Đeo kính điều chỉnh", "Uống thuốc hạ áp, theo dõi",
    "Truyền dịch, nghỉ ngơi", "Bổ sung sắt, vitamin", "Thuốc giảm axit, kiêng đồ chua",
    "Bó bột, nghỉ ngơi", "Bôi kem chống viêm", "Kháng sinh, theo dõi chức năng thận",
    "Nghỉ ngơi, hạ sốt", "Rửa xoang, kháng sinh", "Vật lý trị liệu, giảm đau",
    "Thuốc hạ đường huyết, chế độ ăn", "Bôi kem dưỡng ẩm, tránh kích ứng",
    "Thuốc hỗ trợ tim mạch, nghỉ ngơi", "Kháng sinh, nghỉ ngơi, hỗ trợ hô hấp",
    "Uống nhiều nước, thuốc giảm đau, tán sỏi", "Thuốc kháng virus, theo dõi chức năng gan",
    "Thuốc giảm đau, tránh ánh sáng mạnh", "Thuốc chống viêm, vật lý trị liệu",
    "Thuốc giãn phế quản, tránh tác nhân kích ứng", "Thuốc ức chế bơm proton, kiêng đồ cay",
    "Kháng sinh, uống nhiều nước", "Phẫu thuật cắt ruột thừa, kháng sinh",
    "Thuốc tiêu sợi huyết, phục hồi chức năng"
]

# Danh sách triệu chứng
symptoms = [
    "Đau họng", "Mờ mắt", "Chóng mặt", "Sốt cao", "Mệt mỏi", "Đau thượng vị", "Đau tay",
    "Ngứa da", "Đau lưng dưới", "Hắt hơi", "Nghẹt mũi", "Đau lưng", "Khát nước", "Ngứa da tay",
    "Khó thở", "Đau đầu", "Sốt", "Đau dạ dày", "Đau ngực", "Buồn nôn", "Nôn mửa", "Tiêu chảy",
    "Táo bón", "Đau bụng", "Sưng khớp", "Khó tiêu", "Ho khan", "Ho có đờm", "Đau ngực khi thở",
    "Sưng chân", "Mất ngủ", "Đau vai gáy", "Chán ăn", "Sút cân không rõ nguyên nhân", "Da nhợt nhạt",
    "Mỏi mắt", "Phát ban", "Da khô", "Tiểu khó", "Chảy nước mũi", "Cứng lưng", "Tiểu nhiều",
    "Ợ nóng", "Tiểu buốt", "Đau bụng dưới", "Cứng khớp buổi sáng", "Thở khò khè", "Vàng da",
    "Nhạy cảm với ánh sáng", "Yếu một bên cơ thể", "Khó nói"
]

# Ánh xạ bệnh-triệu chứng
disease_symptoms_map = {
    "Viêm họng cấp": ["Đau họng", "Sốt", "Ho khan"],
    "Cận thị": ["Mờ mắt", "Đau đầu", "Mỏi mắt"],
    "Tăng huyết áp": ["Chóng mặt", "Đau đầu", "Mệt mỏi"],
    "Sốt xuất huyết": ["Sốt cao", "Mệt mỏi", "Đau cơ", "Phát ban"],
    "Thiếu máu": ["Mệt mỏi", "Da nhợt nhạt", "Khó thở"],
    "Viêm dạ dày": ["Đau thượng vị", "Buồn nôn", "Khó tiêu"],
    "Gãy xương cẳng tay": ["Đau tay", "Sưng", "Khó cử động"],
    "Viêm da dị ứng": ["Ngứa da", "Phát ban", "Da khô"],
    "Viêm thận cấp": ["Đau lưng dưới", "Tiểu khó", "Sốt"],
    "Sốt virus": ["Sốt", "Hắt hơi", "Chảy nước mũi", "Mệt mỏi"],
    "Viêm xoang mạn": ["Nghẹt mũi", "Đau đầu", "Chảy nước mũi"],
    "Đau lưng cơ học": ["Đau lưng", "Cứng lưng", "Khó cúi"],
    "Tiểu đường type 2": ["Khát nước", "Tiểu nhiều", "Mệt mỏi", "Sút cân không rõ nguyên nhân"],
    "Chàm (eczema)": ["Ngứa da tay", "Da khô", "Phát ban"],
    "Suy tim nhẹ": ["Khó thở", "Sưng chân", "Mệt mỏi"],
    "Viêm phổi": ["Sốt", "Ho có đờm", "Đau ngực khi thở", "Khó thở"],
    "Sỏi thận": ["Đau lưng dưới", "Tiểu khó", "Đau bụng", "Buồn nôn"],
    "Viêm gan B": ["Mệt mỏi", "Vàng da", "Đau bụng", "Chán ăn"],
    "Đau nửa đầu (migraine)": ["Đau đầu", "Buồn nôn", "Nhạy cảm với ánh sáng"],
    "Viêm khớp dạng thấp": ["Sưng khớp", "Đau khớp", "Cứng khớp buổi sáng"],
    "Hen suyễn": ["Khó thở", "Thở khò khè", "Ho khan"],
    "Loét dạ dày": ["Đau thượng vị", "Ợ nóng", "Buồn nôn", "Khó tiêu"],
    "Nhiễm trùng đường tiểu (UTI)": ["Tiểu khó", "Tiểu buốt", "Đau bụng dưới"],
    "Viêm ruột thừa": ["Đau bụng", "Sốt", "Buồn nôn", "Chán ăn"],
    "Đột quỵ": ["Đau đầu", "Chóng mặt", "Yếu một bên cơ thể", "Khó nói"]
}

# Tạo dữ liệu giả lập
np.random.seed(42)
num_records = 5000  # Số lượng bản ghi giả lập
data = []

# Tạo danh sách triệu chứng làm cột
symptom_cols = sorted(symptoms)

for i in range(num_records):
    # Thông tin cơ bản
    ma_ho_so_y_te = i + 1

    # Chọn ngẫu nhiên một bệnh và phương pháp điều trị tương ứng
    idx = np.random.randint(0, len(diseases))
    chuan_doan = diseases[idx]
    phuong_phap_dieu_tri = treatments[idx]

    # Lấy danh sách triệu chứng liên quan đến bệnh
    related_symptoms = disease_symptoms_map[chuan_doan]
    # Chỉ giữ lại 80% triệu chứng đặc trưng (mô phỏng trường hợp bệnh nhân không có đầy đủ triệu chứng)
    related_symptoms = [s for s in related_symptoms if np.random.random() < 0.8]
    # Thêm 0-2 triệu chứng ngẫu nhiên khác để tăng tính đa dạng
    additional_symptoms = np.random.choice(
        [s for s in symptoms if s not in related_symptoms],
        np.random.randint(0, 3),
        replace=False
    )
    # Thêm triệu chứng không liên quan với xác suất 20%
    if np.random.random() < 0.2:
        unrelated_symptom = np.random.choice([s for s in symptoms if s not in related_symptoms], 1)[0]
        additional_symptoms = list(additional_symptoms) + [unrelated_symptom]
    selected_symptoms = list(related_symptoms) + list(additional_symptoms)

    # Tạo bản ghi
    record = {
        "Mã hồ sơ y tế": ma_ho_so_y_te,
        "Chẩn đoán": chuan_doan,
        "Phương pháp điều trị": phuong_phap_dieu_tri,
    }

    # Thêm các cột triệu chứng
    for symptom in symptom_cols:
        record[f"Triệu chứng_{symptom}"] = 1 if symptom in selected_symptoms else 0

    data.append(record)

# Tạo DataFrame
df = pd.DataFrame(data)

# Mã hóa các cột Chẩn đoán và Phương pháp điều trị
encoded_cols = pd.DataFrame({
    "Mã chẩn đoán": df["Chẩn đoán"].astype("category").cat.codes,
    "Mã phương pháp điều trị": df["Phương pháp điều trị"].astype("category").cat.codes
})
df = pd.concat([df, encoded_cols], axis=1)

# Tối ưu bộ nhớ bằng cách chuyển đổi kiểu dữ liệu
for symptom in symptom_cols:
    df[f"Triệu chứng_{symptom}"] = df[f"Triệu chứng_{symptom}"].astype("int8")

# Lưu ánh xạ
diagnosis_map = dict(enumerate(df["Chẩn đoán"].astype("category").cat.categories))
treatment_map = dict(enumerate(df["Phương pháp điều trị"].astype("category").cat.categories))

with open("../models/diagnosis_map_vn.pkl", "wb") as f:
    pickle.dump(diagnosis_map, f)
with open("../models/treatment_map_vn.pkl", "wb") as f:
    pickle.dump(treatment_map, f)

# Lưu dataset
df.to_csv("../data/medical_data_from_sql.csv", index=False, encoding="utf-8-sig")

print("Đã tạo dataset thành công và lưu tại ../data/medical_data_from_sql.csv")