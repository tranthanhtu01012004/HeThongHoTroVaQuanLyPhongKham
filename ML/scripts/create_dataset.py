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

treatment_medicine_map = {
    "Uống kháng sinh và giảm đau": [
        {"name": "Amoxicillin", "dose": "500mg", "frequency": "3 lần/ngày", "instruction": "Uống sau ăn"},
        {"name": "Paracetamol", "dose": "500mg", "frequency": "4-6 giờ/lần khi đau", "instruction": "Uống khi cần"}
    ],
    "Đeo kính điều chỉnh": [],
    "Uống thuốc hạ áp, theo dõi": [
        {"name": "Amlodipine", "dose": "5mg", "frequency": "1 lần/ngày", "instruction": "Uống tối"}
    ],
    "Truyền dịch, nghỉ ngơi": [
        {"name": "Natri Clorid", "dose": "500ml", "frequency": "1 lần/ngày", "instruction": "Truyền tĩnh mạch"}
    ],
    "Bổ sung sắt, vitamin": [
        {"name": "Ferrous Sulfate", "dose": "100mg", "frequency": "1 lần/ngày", "instruction": "Uống sau ăn"},
        {"name": "Vitamin B12", "dose": "1000mcg", "frequency": "1 lần/ngày", "instruction": "Uống sáng"}
    ],
    "Thuốc giảm axit, kiêng đồ chua": [
        {"name": "Omeprazole", "dose": "20mg", "frequency": "2 lần/ngày", "instruction": "Uống trước ăn"}
    ],
    "Bó bột, nghỉ ngơi": [],
    "Bôi kem chống viêm": [
        {"name": "Hydrocortisone", "dose": "1%", "frequency": "2 lần/ngày", "instruction": "Bôi ngoài da"}
    ],
    "Kháng sinh, theo dõi chức năng thận": [
        {"name": "Ciprofloxacin", "dose": "500mg", "frequency": "2 lần/ngày", "instruction": "Uống sau ăn"}
    ],
    "Nghỉ ngơi, hạ sốt": [
        {"name": "Paracetamol", "dose": "500mg", "frequency": "4-6 giờ/lần khi sốt", "instruction": "Uống khi cần"}
    ],
    "Rửa xoang, kháng sinh": [
        {"name": "Natri Clorid", "dose": "0.9%", "frequency": "2 lần/ngày", "instruction": "Rửa xoang"},
        {"name": "Amoxicillin", "dose": "500mg", "frequency": "3 lần/ngày", "instruction": "Uống sau ăn"}
    ],
    "Vật lý trị liệu, giảm đau": [
        {"name": "Ibuprofen", "dose": "200mg", "frequency": "3 lần/ngày", "instruction": "Uống khi đau"}
    ],
    "Thuốc hạ đường huyết, chế độ ăn": [
        {"name": "Metformin", "dose": "500mg", "frequency": "2 lần/ngày", "instruction": "Uống sau ăn"}
    ],
    "Bôi kem dưỡng ẩm, tránh kích ứng": [
        {"name": "Cetaphil", "dose": "Lượng vừa đủ", "frequency": "2 lần/ngày", "instruction": "Bôi ngoài da"}
    ],
    "Thuốc hỗ trợ tim mạch, nghỉ ngơi": [
        {"name": "Furosemide", "dose": "40mg", "frequency": "1 lần/ngày", "instruction": "Uống sáng"}
    ],
    "Kháng sinh, nghỉ ngơi, hỗ trợ hô hấp": [
        {"name": "Azithromycin", "dose": "500mg", "frequency": "1 lần/ngày", "instruction": "Uống trước ăn"}
    ],
    "Uống nhiều nước, thuốc giảm đau, tán sỏi": [
        {"name": "Spasmolyt", "dose": "10mg", "frequency": "2 lần/ngày", "instruction": "Uống khi đau"}
    ],
    "Thuốc kháng virus, theo dõi chức năng gan": [
        {"name": "Tenofovir", "dose": "300mg", "frequency": "1 lần/ngày", "instruction": "Uống tối"}
    ],
    "Thuốc giảm đau, tránh ánh sáng mạnh": [
        {"name": "Sumatriptan", "dose": "50mg", "frequency": "1 lần khi đau", "instruction": "Uống khi cần"}
    ],
    "Thuốc chống viêm, vật lý trị liệu": [
        {"name": "Diclofenac", "dose": "50mg", "frequency": "2 lần/ngày", "instruction": "Uống sau ăn"}
    ],
    "Thuốc giãn phế quản, tránh tác nhân kích ứng": [
        {"name": "Salbutamol", "dose": "2mg", "frequency": "3 lần/ngày", "instruction": "Uống khi cần"}
    ],
    "Thuốc ức chế bơm proton, kiêng đồ cay": [
        {"name": "Pantoprazole", "dose": "40mg", "frequency": "1 lần/ngày", "instruction": "Uống trước ăn"}
    ],
    "Kháng sinh, uống nhiều nước": [
        {"name": "Ciprofloxacin", "dose": "500mg", "frequency": "2 lần/ngày", "instruction": "Uống sau ăn"}
    ],
    "Phẫu thuật cắt ruột thừa, kháng sinh": [
        {"name": "Ceftriaxone", "dose": "1g", "frequency": "1 lần/ngày", "instruction": "Tiêm tĩnh mạch"}
    ],
    "Thuốc tiêu sợi huyết, phục hồi chức năng": [
        {"name": "Alteplase", "dose": "0.9mg/kg", "frequency": "1 lần", "instruction": "Tiêm tĩnh mạch"}
    ]
}


# Tạo danh sách tất cả các thuốc duy nhất
all_medicines = sorted(set(med["name"] for treatment in treatment_medicine_map.values() for med in treatment))

# Tạo dữ liệu giả lập
np.random.seed(42)
num_records = 5000
data = []
symptom_cols = sorted(symptoms)

for i in range(num_records):
    ma_ho_so_y_te = i + 1
    idx = np.random.randint(0, len(diseases))
    chuan_doan = diseases[idx]
    phuong_phap_dieu_tri = treatments[idx]

    # Lấy triệu chứng (chỉ chọn 1-3 triệu chứng để tăng tính đa dạng)
    related_symptoms = disease_symptoms_map[chuan_doan]
    num_symptoms = np.random.randint(1, min(4, len(related_symptoms) + 1))  # Chọn ngẫu nhiên 1-3 triệu chứng
    selected_symptoms = np.random.choice(related_symptoms, num_symptoms, replace=False).tolist()

    # Thêm triệu chứng không liên quan (ít hơn)
    if np.random.random() < 0.1:  # Giảm xác suất xuống 10%
        unrelated_symptom = np.random.choice([s for s in symptoms if s not in related_symptoms], 1)[0]
        selected_symptoms.append(unrelated_symptom)

    # Tạo bản ghi
    record = {
        "Mã hồ sơ y tế": ma_ho_so_y_te,
        "Chẩn đoán": chuan_doan,
        "Phương pháp điều trị": phuong_phap_dieu_tri,
    }
    for symptom in symptom_cols:
        record[f"Triệu chứng_{symptom}"] = 1 if symptom in selected_symptoms else 0
    for medicine in all_medicines:
        record[f"Thuốc_{medicine}"] = 1 if medicine in [m["name"] for m in treatment_medicine_map[phuong_phap_dieu_tri]] else 0
    data.append(record)
    
# Tạo DataFrame
df = pd.DataFrame(data)

# Mã hóa các cột Chẩn đoán và Phương pháp điều trị
encoded_cols = pd.DataFrame({
    "Mã chẩn đoán": df["Chẩn đoán"].astype("category").cat.codes,
    "Mã phương pháp điều trị": df["Phương pháp điều trị"].astype("category").cat.codes
})
df = pd.concat([df, encoded_cols], axis=1)

# Tối ưu bộ nhớ
for symptom in symptom_cols:
    df[f"Triệu chứng_{symptom}"] = df[f"Triệu chứng_{symptom}"].astype("int8")
for medicine in all_medicines:
    df[f"Thuốc_{medicine}"] = df[f"Thuốc_{medicine}"].astype("int8")

# Lưu ánh xạ
diagnosis_map = dict(enumerate(df["Chẩn đoán"].astype("category").cat.categories))
treatment_map = dict(enumerate(df["Phương pháp điều trị"].astype("category").cat.categories))

with open("../models/diagnosis_map_vn.pkl", "wb") as f:
    pickle.dump(diagnosis_map, f)
with open("../models/treatment_map_vn.pkl", "wb") as f:
    pickle.dump(treatment_map, f)
with open("../models/medicine_map.pkl", "wb") as f:
    pickle.dump(treatment_medicine_map, f)  # Lưu ánh xạ thuốc để dùng sau

# Lưu dataset
df.to_csv("../data/medical_data_from_sql.csv", index=False, encoding="utf-8-sig")
print("Đã tạo dataset thành công và lưu tại ../data/medical_data_from_sql.csv")