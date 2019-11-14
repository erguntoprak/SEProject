import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms';
import { v4 as uuid } from 'uuid';
import { BaseService } from '../../../../shared/base.service';
import { ResponseModel } from '../../../../shared/response-model';
@Component({
    selector: 'se-education-create',
    templateUrl: './education-create.component.html'
})
export class EducationCreateComponent implements OnInit {
    educationForm: FormGroup;
    errorList = [];
    submitted = false;
    category: object;
    attributeList: object;
    urlImages: KeyValueModel[] = [];

    constructor(private formBuilder: FormBuilder, private baseService: BaseService) { }

    ngOnInit() {
        this.getAllCallMethod();
        //@ts-ignore
        CKEDITOR.replace('editor1');
        this.educationForm = this.formBuilder.group({
            educationName: ['asdasd', Validators.required],
            educationType: [0, Validators.required],
            description: ['', Validators.required],
            attributes: this.formBuilder.array([])
        });
    }

    onSubmit() {
        debugger;
        this.submitted = true;
        //@ts-ignore
        this.educationForm.get('description').setValue(CKEDITOR.instances.editor1.getData());

        if (this.educationForm.invalid) {
            return;
        }

        // display form values on success
        alert('SUCCESS!! :-)\n\n' + JSON.stringify(this.educationForm.value, null, 4));
    }

    //remove selected image
    removeImage(id) {
        this.urlImages = this.urlImages.filter(el => el.key !== id);
        var deleteImage = document.getElementById(id);
        deleteImage.remove();
    }
    //select image
    onSelectFile(event) {
        if (event.target.files && event.target.files[0]) {
            var filesAmount = event.target.files.length;
            for (let i = 0; i < filesAmount; i++) {
                var reader = new FileReader();

                reader.onload = (event: any) => {
                    this.urlImages.push({ key: uuid(), value: event.target.result });
                }

                reader.readAsDataURL(event.target.files[i]);
            }
        }
    }
    //Checkbox change checked type
    onChange(id: string, isChecked: boolean) {
        const attributes = <FormArray>this.educationForm.controls.attributes;

        if (isChecked) {
            attributes.push(new FormControl(id));
        } else {
            let index = attributes.controls.findIndex(x => x.value == id)
            attributes.removeAt(index);
        }
    }
    //Gel All
    getAllCallMethod() {
        this.baseService.getAll<ResponseModel>("Attribute/GetAllAttributeList").subscribe(responseModel => {
            this.attributeList = responseModel.data;
            if (responseModel.errorMessage.length > 0) {
                this.errorList = this.errorList.concat(responseModel.errorMessage);
            }
        });
        this.baseService.getAll<ResponseModel>("Category/GetAllCategoryList").subscribe(responseModel => {
            this.category = responseModel.data;
            if (responseModel.errorMessage.length > 0) {
                this.errorList = this.errorList.concat(responseModel.errorMessage);
            }
        });
    }
}
