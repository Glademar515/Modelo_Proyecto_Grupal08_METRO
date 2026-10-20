using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo_Proyecto_Grupal
{
    public class ValidadorSeguridad
    {
        private const int PIN_SUPERVISOR = 1234;

        public bool ValidarCantidad(int cantidad)
        {
            return cantidad > 0;
        }

        public bool ValidarRestriccionEdad(int esAlcohol)
        {
            if (esAlcohol == 1)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\n[ALERTA DE SEGURIDAD] Producto con restricción de edad (+18).");
                Console.ResetColor();
                Console.Write("Solicite al supervisor ingresar PIN de autorización (1234): ");

                int pinIngresado;
                bool esNumero = int.TryParse(Console.ReadLine(), out pinIngresado);

                if (esNumero && pinIngresado == PIN_SUPERVISOR)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("-> Autorización concedida por el supervisor.");
                    Console.ResetColor();
                    return true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("-> PIN incorrecto. Se rechaza el registro del producto.");
                    Console.ResetColor();
                    return false;
                }
            }
            return true;
        }

        public bool ValidarDocumento(string documento, int tipoComprobante)
        {
            if (tipoComprobante == 1 && documento.Length == 8) return true;
            if (tipoComprobante == 2 && documento.Length == 11) return true;
            return false;
        }
    }
}