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
  id: number;
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
export interface ImageModel {
  id: number;
  imageUrl: string;
  title: string;
  firstVisible: boolean;
  educationId: number;
}
export interface EducationContactFormInsertModel {
  nameSurname: string;
  email: string;
  phoneNumber: string;
  educationId: number;
  createDateTime: Date;
}
export interface EducationContactFormListModel {
  nameSurname: string;
  email: string;
  phoneNumber: string;
  createDateTime: string;
}
export interface BlogListModel {
  id: number;
  title: string;
  author: string;
  seoUrl: string;
  firstVisibleImageName: string;
  createTime: string;
}
export interface BlogDetailModel {
  id: number;
  title: string;
  author: string;
  firstVisibleImageName: string;
  createTime: string;
  blogItems: BlogItemModel[];
}
export interface BlogItemModel {
  imageName: string;
  description: string;
}
export interface BlogUpdateModel {
  id: number;
  title: string;
  firstVisibleImageName: string;
  blogItems: BlogItemModel[];
}
