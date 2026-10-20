using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo_Proyecto_Grupal
{
    public class ControladorPagos
    {
        private const double TASA_IGV = 0.18;
        // Matriz bidimensional 2D [20 filas, 4 columnas] exigida en el TEMA 03
        private double[,] matrizTransacciones = new double[20, 4];
        private int totalTransaccionesTurno = 0;

        public void ProcesarPagoYComprobante(
            string[] codigos, string[] nombres, double[] precios,
            int[] cantidades, double[] subtotales, ref int totalItems,
            ValidadorSeguridad validador)
        {
            if (totalItems == 0)
            {
                Console.WriteLine("\nLa canasta está vacía. No hay nada que pagar.");
                return;
            }

            double totalBruto = 0;
            for (int i = 0; i < totalItems; i++) totalBruto += subtotales[i];

            double descuento = (totalBruto > 150.0) ? totalBruto * 0.05 : 0.0;
            double totalPagar = totalBruto - descuento;
            double opGravada = totalPagar / (1.0 + TASA_IGV);
            double igv = totalPagar - opGravada;
            int puntosBonus = (int)(totalPagar / 10);

            Console.Write("\nSeleccione tipo de comprobante (1: Boleta con DNI / 2: Factura con RUC): ");
            int tipoComp = Convert.ToInt32(Console.ReadLine());

            Console.Write("Ingrese número de documento: ");
            string docCliente = Console.ReadLine();

            Console.Write("Ingrese últimos 4 dígitos de su tarjeta: ");
            string numTarjeta = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n=====================================================");
            Console.WriteLine("        SUPERMERCADOS METRO - CENCOSUD PERÚ          ");
            Console.WriteLine("          COMPROBANTE DE PAGO ELECTRÓNICO            ");
            Console.WriteLine("=====================================================");
            Console.ResetColor();
            Console.WriteLine($"Tipo Comprobante  : {(tipoComp == 2 ? "FACTURA" : "BOLETA")}");
            Console.WriteLine($"Documento Cliente : {docCliente}");
            Console.WriteLine($"Medio de Pago     : Tarjeta ****{numTarjeta}");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine($"Operación Gravada : S/ {opGravada:F2}");
            Console.WriteLine($"Descuento (5%)    : S/ {descuento:F2}");
            Console.WriteLine($"IGV (18%)         : S/ {igv:F2}");
            Console.WriteLine($"IMPORTE TOTAL     : S/ {totalPagar:F2}");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine($"Puntos Bonus      : {puntosBonus} pts.");
            Console.WriteLine($"Estado            : APROBADO - HASH #MTR2026T{totalTransaccionesTurno + 1}");
            Console.WriteLine("=====================================================");

            // Guardar en la matriz bidimensional
            if (totalTransaccionesTurno < 20)
            {
                matrizTransacciones[totalTransaccionesTurno, 0] = opGravada;
                matrizTransacciones[totalTransaccionesTurno, 1] = descuento;
                matrizTransacciones[totalTransaccionesTurno, 2] = igv;
                matrizTransacciones[totalTransaccionesTurno, 3] = totalPagar;
                totalTransaccionesTurno++;
            }

            totalItems = 0; // Limpiar canasta
        }

        public void MostrarMatrizAuditoriaPagos()
        {
            if (totalTransaccionesTurno == 0)
            {
                Console.WriteLine("\nNo hay transacciones registradas en este turno.");
                return;
            }

            Console.WriteLine("\n=====================================================================");
            Console.WriteLine("   MATRIZ BIDIMENSIONAL DE AUDITORÍA DE PAGOS (CAJA METRO)           ");
            Console.WriteLine("=====================================================================");
            Console.WriteLine($"{"TICKET",-10} | {"OP. GRAVADA",-14} | {"DESCUENTO",-12} | {"IGV (18%)",-12} | {"TOTAL PAGADO"}");
            Console.WriteLine("---------------------------------------------------------------------");

            double totalCaja = 0;
            for (int i = 0; i < totalTransaccionesTurno; i++)
            {
                Console.WriteLine($"Ticket #{i + 1,-3} | S/ {matrizTransacciones[i, 0],-11:F2} | S/ {matrizTransacciones[i, 1],-9:F2} | S/ {matrizTransacciones[i, 2],-9:F2} | S/ {matrizTransacciones[i, 3]:F2}");
                totalCaja += matrizTransacciones[i, 3];
            }
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine($"RECAUDACIÓN TOTAL EN CAJA: S/ {totalCaja:F2}");
        }
    }
}