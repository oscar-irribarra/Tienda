using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tienda.Models
{
    /// <summary>
	/// Clase para obtener valores de clase tbk-normal
	/// </summary>
    public class ResponseModel
    {       
        public string Message { get; set; }
        /// <summary>
        /// Respuesta de los metodos de tbk [ correcto (try) = true, error (catch) = false   ]    
        /// </summary>
        public bool Response { get; set; }
        public string Result { get; set; }
        public string Request { get; set; }
        /// <summary>
        /// Url Generada por transkbank [pagina de pago]   
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// -> Token obtenido de transbank 
        /// [Guardar al recibir el valor en el controlador]
        /// </summary>
        public string Token { get; set; }
    }

    /// <summary>
	/// Clase para enviar valores a clase tbk-normal
	/// </summary>
    public class ResponseModelcomercio
    {
        /// <summary>
        /// Especifica accion de webpay init, result, end, nullify
        /// </summary>
        public string Action { get; set; }
        public string Commercecode { get; set; }
        /// <summary>
        /// Monto total de la transacción
        /// </summary>
        public string AuthorizedAmount { get; set; }
        public string AuthorizationCode { get; set; }
        public string BuyOrder { get; set; }
        /// <summary>
        /// -> Token devuelto a la clase tbk-normal
        /// </summary>
        public string Tokentransaccion { get; set; }
        public string SessionId { get; set; }
        /// <summary>
        /// Url a la cual direccionara cuando se inicia la transacción
        /// </summary>
        public string Urlreturn { get; set; }
        /// <summary>
        /// Url a la cual direccionara despues confirmar la transacción
        /// </summary>
        public string Urlfin { get; set; }
        /// <summary>
        /// Url a la cual direccionara despues de imprimir el voucher de webpay
        /// </summary>
        public string Urlend { get; set; }
        /// <summary>
        /// Monto por el cual se anulara la transaccion
        /// </summary>
        public string NullifyAmount { get; set; }
    }
}