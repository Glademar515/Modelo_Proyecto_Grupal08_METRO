using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo_Proyecto_Grupal
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] codigos = new string[50];
            string[] nombres = new string[50];
            double[] precios = new double[50];
            int[] cantidades = new int[50];
            double[] subtotales = new double[50];
            int totalItems = 0;

            ValidadorSeguridad validador = new ValidadorSeguridad();
            GestorCanasta gestorCanasta = new GestorCanasta();
            ControladorPagos controladorPagos = new ControladorPagos();

            int opcion = -1;
            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n==============================================================");
                Console.WriteLine("   SISTEMA DE CONTROL DE PAGOS Y AUTOCOBRO - METRO LIMA 2026  ");
                Console.WriteLine("==============================================================");
                Console.ResetColor();
                Console.WriteLine("1. Ver catálogo y registrar/escanear producto");
                Console.WriteLine("2. Buscar producto en la canasta");
                Console.WriteLine("3. Modificar cantidad de un producto");
                Console.WriteLine("4. Eliminar producto de la canasta");
                Console.WriteLine("5. Mostrar canasta actual");
                Console.WriteLine("6. Ordenar canasta por Precio Unitario (Burbuja)");
                Console.WriteLine("7. Ordenar canasta por Subtotal (Selección)");
                Console.WriteLine("8. Procesar pago y emitir comprobante");
                Console.WriteLine("9. Ver reporte de auditoría");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion)) opcion = -1;

                switch (opcion)
                {
                    case 1:
                        gestorCanasta.RegistrarProductoEscaneado(codigos, nombres, precios, cantidades, subtotales, ref totalItems, validador);
                        break;
                    case 2:
                        gestorCanasta.BuscarProductoEnCanasta(codigos, nombres, precios, cantidades, subtotales, totalItems);
                        break;
                    case 3:
                        gestorCanasta.ModificarCantidad(codigos, nombres, precios, cantidades, subtotales, totalItems, validador);
                        break;
                    case 4:
                        gestorCanasta.EliminarProducto(codigos, nombres, precios, cantidades, subtotales, ref totalItems);
                        break;
                    case 5:
                        gestorCanasta.MostrarCanasta(codigos, nombres, precios, cantidades, subtotales, totalItems);
                        break;
                    case 6:
                        gestorCanasta.OrdenarBurbujaPorPrecio(codigos, nombres, precios, cantidades, subtotales, totalItems);
                        break;
                    case 7:
                        gestorCanasta.OrdenarSeleccionPorSubtotal(codigos, nombres, precios, cantidades, subtotales, totalItems);
                        break;
                    case 8:
                        controladorPagos.ProcesarPagoYComprobante(codigos, nombres, precios, cantidades, subtotales, ref totalItems, validador);
                        break;
                    case 9:
                        controladorPagos.MostrarMatrizAuditoriaPagos();
                        break;
                    case 0:
                        Console.WriteLine("Saliendo del sistema...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            } while (opcion != 0);
        }
    }
}