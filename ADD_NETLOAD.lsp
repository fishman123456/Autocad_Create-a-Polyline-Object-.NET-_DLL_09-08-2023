(defun C:ADD_NETLOAD (/ x1 x2 x3)
					;загружаем DLL 
  (vl-load-com)
  ;(vl-cmdf "_netload"  "C:\Users\Fishman.DB.CORP\Documents\GitHub\Autocad_Create a Polyline Object (.NET)_DLL_09-08-2023\bin\Debug\Autocad_Create a Polyline Object (.NET)_DLL_09-08-2023.dll") 
 (vl-cmdf "_netload" "C:/Users/Fishman.DB.CORP/Documents/GitHub/Autocad_Create a Polyline Object (.NET)_DLL_09-08-2023/bin/Debug/Autocad_Create a Polyline Object (.NET)_DLL_09-08-2023.dll")
  ;- лиспом dll загружаем
  (alert "DLL Загружен")
)