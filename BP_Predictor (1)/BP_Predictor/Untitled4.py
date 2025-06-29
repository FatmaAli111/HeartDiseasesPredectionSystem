
import pandas as pd
import numpy as np
from sklearn.model_selection import train_test_split, GridSearchCV
from sklearn.preprocessing import StandardScaler
from sklearn.impute import KNNImputer
from sklearn.multioutput import MultiOutputRegressor
from sklearn.ensemble import GradientBoostingRegressor
from sklearn.metrics import mean_squared_error, r2_score
import joblib

# تحميل البيانات
fhs_data = pd.read_csv(r'C:\Users\AB\Downloads\BP_Predictor (1)\BP_Predictor\frmgham2.csv')

# إزالة الصفوف التي تحتوي على قيم ناقصة
fhs_data.dropna(axis=0, inplace=True)

#  إزالة SNPs ذات تردد أليل أقل من 5% (لو عندك أعمدة SNPs فعلًا)
maf_threshold = 0.05
minor_allele_frequency = fhs_data.mean(axis=0) / 2
common_snps = minor_allele_frequency[minor_allele_frequency >= maf_threshold].index
fhs_data = fhs_data[common_snps]

# إذا لم يكن لديك أعمدة SNPs، تجاهل هذا الجزء!

# ملء القيم الناقصة (إن وجدت بعد الفلترة)
imputer = KNNImputer(n_neighbors=5)
fhs_data = pd.DataFrame(imputer.fit_transform(fhs_data), columns=fhs_data.columns)

# إعداد المميزات والهدف
# الهدف: SYSBP و DIABP
targets = fhs_data[['SYSBP', 'DIABP']]
features = fhs_data.drop(['SYSBP', 'DIABP'], axis=1)

# تقسيم البيانات
X_train, X_test, y_train, y_test = train_test_split(features, targets, test_size=0.2, random_state=42)

# قياس المميزات
scaler = StandardScaler()
X_train_scaled = scaler.fit_transform(X_train)
X_test_scaled = scaler.transform(X_test)

# بناء الموديل متعدد المخرجات
gbr = GradientBoostingRegressor()

multi_output_model = MultiOutputRegressor(gbr)

# بحث GridSearch لأفضل الهايبر باراميترز
param_grid = {
    'estimator__n_estimators': [100, 300],
    'estimator__learning_rate': [0.01, 0.1],
    'estimator__max_depth': [3, 5]
}

grid_search = GridSearchCV(multi_output_model, param_grid=param_grid, cv=3, n_jobs=-1)
grid_search.fit(X_train_scaled, y_train)

print("Best parameters:", grid_search.best_params_)
print("Best score:", grid_search.best_score_)

# تقييم الموديل
y_pred = grid_search.predict(X_test_scaled)

mse_sysbp = mean_squared_error(y_test['SYSBP'], y_pred[:, 0])
r2_sysbp = r2_score(y_test['SYSBP'], y_pred[:, 0])

mse_diabp = mean_squared_error(y_test['DIABP'], y_pred[:, 1])
r2_diabp = r2_score(y_test['DIABP'], y_pred[:, 1])

print(f"SYSBP - MSE: {mse_sysbp}, R2: {r2_sysbp}")
print(f"DIABP - MSE: {mse_diabp}, R2: {r2_diabp}")

# حفظ الموديل والسكيلر
joblib.dump(grid_search.best_estimator_, 'best_model.pkl')
joblib.dump(scaler, 'scaler.pkl')

# حفظ الأعمدة المتوقعة (feature names)
expected_features = features.columns.tolist()
joblib.dump(expected_features, 'expected_features.pkl')







