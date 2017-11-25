
    jQuery.extend(jQuery.validator.messages, {
        required: "Este campo es Obligatorio.",
    remote: "Arregle este Campo.",
    email: "Ingrese un correo valido.",
    url: "Ingrese una Url Valida.",
    date: "Ingrese una Fecha Valida.",
    dateISO: "Ingrese una Fecha Validae (ISO).",
    number: "Ingrese solo numeros.",
    digits: "Ingrese solo Digitos.",
    creditcard: "Ingrese un numero de tarjeta valido.",
    equalTo: "Ingrese el valor nuevamente.",
    maxlength: jQuery.validator.format("Solo puede ingresar como maximo {0} Caracteres."),
    minlength: jQuery.validator.format("Solo puede ingresar como minimo {0} Caracteres."),
    rangelength: jQuery.validator.format("Ingrese solo cadenas entre {0} y {1} caracteres."),
    range: jQuery.validator.format("Ingrese solo numeros entre {0} y {1}."),
    max: jQuery.validator.format("Ingrese solo numeros igual o menores a {0}."),
    min: jQuery.validator.format("Ingrese solo numeros igual o mayores a {0}.")
});