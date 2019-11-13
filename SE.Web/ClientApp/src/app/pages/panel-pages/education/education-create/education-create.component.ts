import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms';
import { v4 as uuid } from 'uuid';
import * as $ from 'jquery';
import { BaseService } from '../../../../shared/base.service';
@Component({
    selector: 'se-education-create',
    templateUrl: './education-create.component.html'
})
export class EducationCreateComponent implements OnInit {
    educationForm: FormGroup;
    submitted = false;
    category : any;
    attributeList: any;
    physicalFacilities = [{ "Id": 1, "Name": "Deneme1","Selected":true},
        { "Id": 2, "Name": "Deneme2", "Selected": true },
        { "Id": 3, "Name": "Deneme3", "Selected": true },
        { "Id": 4, "Name": "Deneme4", "Selected": true },
        { "Id": 5, "Name": "Deneme5", "Selected": true }
    ];
    urlImages: KeyValueModel[] = [];

    constructor(private formBuilder: FormBuilder, private baseService: BaseService) { }

    removeImage(id) {
        this.urlImages = this.urlImages.filter(el => el.key !== id);
        var deleteImage = document.getElementById(id);
        deleteImage.remove();
    }
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


    ngOnInit() {
        this.baseService.getAll("Attribute/GetAllAttributeList").subscribe(data => {
            this.attributeList = data;
        });
        this.baseService.getAll("Category/GetAllCategoryList").subscribe(data => {
            this.category = data;
        });
        $(document).ready(function () {
            //@ts-ignore
            CKEDITOR.replace('editor1');
        });
        this.educationForm = this.formBuilder.group({
            educationName: ['asdasd', Validators.required],
            educationType: [0, Validators.required],
            description: ['', Validators.required],
            attributes: this.formBuilder.array([])
        });
    }
    onChange(id: string, isChecked: boolean) {
        const attributes = <FormArray>this.educationForm.controls.attributes;

        if (isChecked) {
            attributes.push(new FormControl(id));
        } else {
            let index = attributes.controls.findIndex(x => x.value == id)
            attributes.removeAt(index);
        }
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
}
