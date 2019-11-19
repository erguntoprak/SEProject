"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
function PhoneNumberValidator(control) {
    var valid = /^\d+$/.test(control.value);
    return valid
        ? null
        : { invalidNumber: { valid: false, value: control.value } };
}
exports.PhoneNumberValidator = PhoneNumberValidator;
//# sourceMappingURL=phone-number.validator.js.map