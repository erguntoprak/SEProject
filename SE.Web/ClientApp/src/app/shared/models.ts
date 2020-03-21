export interface LoginModel {
    email: string;
    password: string;
}
export interface RegisterModel {
    email: string;
    phone: string;
    password: string;
}
export interface UserModel {
    id: string;
    name: string;
    surname: string;
    token: string;
}
export interface KeyValueModel {
    key: any;
    value: any;
}
export interface EducationListModel {
  name: string;
  categoryName: string;
  categorySeoUrl: string;
  districtName: string;
  address: string;
  base64Image: string;
  seoUrl: string;
}
export interface CategoryAttributeListModel {
  categoryName: string;
  attributeListModel: AttributeModel[];
}

export interface AttributeModel {
  id: string;
  name: string;
}
export interface CategoryModel {
  id: string;
  name: string;
}
export interface AddressModel {
  cityModel: CityModel;
  districtListModel: DistrictModel[];
}
export interface CityModel {
  id: string;
  name: string;
}
export interface DistrictModel {
  id: string;
  name: string;
}
