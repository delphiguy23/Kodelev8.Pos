using System.ComponentModel;

namespace Web.Wasm.Point.Of.Sale.Services.Enum;

public enum Gender
{
    [Description("Undefined")] Undefined = 1,
    [Description("Male")] Male,
    [Description("Female")] Female,
}
