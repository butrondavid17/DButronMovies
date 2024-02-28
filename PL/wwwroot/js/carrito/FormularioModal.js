$(document).ready(function () {
    GetSubTotal();
    GetTotal();
});

function ClearForm() {
    $('#txtNombreCompleto').val('');
    $('#txtDireccion').val('');
    $('#txtTarjeta').val('');
    $('#txtFecha').val('');
    $('#txtCVV').val('');
}
function ShowModalForm() {
    ClearForm();
    $('#Form').modal('show');
}
function ShowModalSuccess() {
    ClearForm();
    $('#Form').modal('hide');
    $('#Success').modal('show');
}
function GetSubTotal() {
    $('#rowProduct td').each(function () {
        var cantidad = parseFloat($('#txtCantidad').text())
        var precio = parseFloat($('#txtPrecio').text())
        var subtotal = cantidad * precio;
        $('#txtSubtotal').text(subtotal);
    })
}
function GetTotal() {
    var sum = 0;
    $('#txtSubtotalsubtotal').each(function () {
        sum += parseFloat($(this).text());
    });
    $('#txtTotal').val(sum.toFixed(2));
}