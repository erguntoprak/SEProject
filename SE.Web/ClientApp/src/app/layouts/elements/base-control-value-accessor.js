"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var BaseControlValueAccessor = /** @class */ (function () {
    function BaseControlValueAccessor() {
        this.disabled = false;
    }
    /**
     * Call when value has changed programmatically
     */
    BaseControlValueAccessor.prototype.onChange = function (newVal) { };
    BaseControlValueAccessor.prototype.onTouched = function (_) { };
    /**
     * Model -> View changes
     */
    BaseControlValueAccessor.prototype.writeValue = function (obj) { this.value = obj; };
    BaseControlValueAccessor.prototype.registerOnChange = function (fn) { this.onChange = fn; };
    BaseControlValueAccessor.prototype.registerOnTouched = function (fn) { this.onTouched = fn; };
    BaseControlValueAccessor.prototype.setDisabledState = function (isDisabled) { this.disabled = isDisabled; };
    return BaseControlValueAccessor;
}());
exports.BaseControlValueAccessor = BaseControlValueAccessor;
//# sourceMappingURL=base-control-value-accessor.js.map