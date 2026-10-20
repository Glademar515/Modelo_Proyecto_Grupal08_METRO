using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo_Proyecto_Grupal
{
    public class GestorCanasta
    {
        // ----------------------------------------------------
        // CATÁLOGO MAESTRO PRECARGADO (Precios y nombres fijos)
        // ----------------------------------------------------
        private string[] catalogoCodigos = { "101", "102", "103" };
        private string[] catalogoNombres = { "Agua San Mateo 600ml", "Leche Gloria Azul 400g", "Cerveza Cusqueña 330ml" };
        private double[] catalogoPrecios = { 2.50, 4.80, 7.50 };
        private int[] catalogoRestriccion = { 0, 0, 1 }; // 1 significa que requiere +18 (Cerveza)

        public void MostrarCatalogoTienda()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\n================== CATÁLOGO DE PRODUCTOS METRO ==================");
            Console.WriteLine($"{"CÓDIGO",-8} | {"PRODUCTO",-26} | {"PRECIO (S/)",-12} | {"RESTRICCIÓN"}");
            Console.WriteLine("-----------------------------------------------------------------");
            for (int i = 0; i < catalogoCodigos.Length; i++)
            {
                string restriccionTexto = (catalogoRestriccion[i] == 1) ? "Sí (+18)" : "Ninguna";
                Console.WriteLine($"{catalogoCodigos[i],-8} | {catalogoNombres[i],-26} | S/ {catalogoPrecios[i],-9:F2} | {restriccionTexto}");
            }
            Console.WriteLine("=================================================================");
            Console.ResetColor();
        }

        public int BuscarEnCatalogo(string codigoBuscado)
        {
            for (int i = 0; i < catalogoCodigos.Length; i++)
            {
                if (catalogoCodigos[i] == codigoBuscado) return i;
            }
            return -1;
        }

        public int BuscarIndiceEnCanasta(string[] codigosCanasta, int totalItems, string codigoBuscado)
        {
            for (int i = 0; i < totalItems; i++)
            {
                if (codigosCanasta[i] == codigoBuscado) return i;
            }
            return -1;
        }

        // Registrar escaneando código de barras del catálogo
        public void RegistrarProductoEscaneado(
            string[] codigos, string[] nombres, double[] precios,
            int[] cantidades, double[] subtotales, ref int totalItems,
            ValidadorSeguridad validador)
        {
            if (totalItems >= codigos.Length)
            {
                Console.WriteLine("Error: La canasta alcanzó su capacidad máxima.");
                return;
            }

            MostrarCatalogoTienda();
            Console.Write("\n[ESCANER] Ingrese o escriba el código de barras del producto: ");
            string codigoEscaneado = Console.ReadLine();

            int indexCatalogo = BuscarEnCatalogo(codigoEscaneado);
            if (indexCatalogo == -1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Código de barras no encontrado en el sistema de Metro.");
                Console.ResetColor();
                return;
            }

            Console.Write($"Producto detectado: {catalogoNombres[indexCatalogo]} (S/ {catalogoPrecios[indexCatalogo]:F2})\nIngrese la cantidad a llevar: ");
            int cant;
            if (!int.TryParse(Console.ReadLine(), out cant) || !validador.ValidarCantidad(cant))
            {
                Console.WriteLine("Error: Cantidad no válida.");
                return;
            }

            // Validar restricción si el producto es alcohol (Cerveza)
            if (validador.ValidarRestriccionEdad(catalogoRestriccion[indexCatalogo]))
            {
                // Verificar si ya estaba en la canasta para sumar unidades o agregarlo como nueva línea
                int posExistente = BuscarIndiceEnCanasta(codigos, totalItems, codigoEscaneado);
                if (posExistente != -1)
                {
                    cantidades[posExistente] += cant;
                    subtotales[posExistente] = precios[posExistente] * cantidades[posExistente];
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("-> El producto ya estaba en la canasta. Se actualizó la cantidad.");
                    Console.ResetColor();
                }
                else
                {
                    codigos[totalItems] = catalogoCodigos[indexCatalogo];
                    nombres[totalItems] = catalogoNombres[indexCatalogo];
                    precios[totalItems] = catalogoPrecios[indexCatalogo];
                    cantidades[totalItems] = cant;
                    subtotales[totalItems] = catalogoPrecios[indexCatalogo] * cant;

                    totalItems++;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("-> Producto agregado exitosamente a la canasta.");
                    Console.ResetColor();
                }
            }
        }

        public void BuscarProductoEnCanasta(string[] codigos, string[] nombres, double[] precios, int[] cantidades, double[] subtotales, int totalItems)
        {
            Console.Write("\nIngrese el código de barras a buscar en su canasta: ");
            string codBuscado = Console.ReadLine();

            int pos = BuscarIndiceEnCanasta(codigos, totalItems, codBuscado);
            if (pos != -1)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"[Encontrado] {nombres[pos]} | Precio Unit: S/ {precios[pos]:F2} | Cant: {cantidades[pos]} | Subtotal: S/ {subtotales[pos]:F2}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("El producto no se encuentra en la canasta actual.");
            }
        }

        public void ModificarCantidad(string[] codigos, string[] nombres, double[] precios, int[] cantidades, double[] subtotales, int totalItems, ValidadorSeguridad validador)
        {
            Console.Write("\nIngrese el código de barras del producto a modificar: ");
            string codMod = Console.ReadLine();

            int pos = BuscarIndiceEnCanasta(codigos, totalItems, codMod);
            if (pos != -1)
            {
                Console.WriteLine($"Producto: {nombres[pos]} | Cantidad actual: {cantidades[pos]}");
                Console.Write("Ingrese la nueva cantidad: ");
                int nuevaCant;
                if (int.TryParse(Console.ReadLine(), out nuevaCant) && validador.ValidarCantidad(nuevaCant))
                {
                    cantidades[pos] = nuevaCant;
                    subtotales[pos] = precios[pos] * nuevaCant;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("-> Cantidad actualizada con éxito.");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("Error: Cantidad no válida.");
                }
            }
            else
            {
                Console.WriteLine("Producto no encontrado en la canasta.");
            }
        }

        public void EliminarProducto(string[] codigos, string[] nombres, double[] precios, int[] cantidades, double[] subtotales, ref int totalItems)
        {
            Console.Write("\nIngrese el código de barras del producto a eliminar: ");
            string codElim = Console.ReadLine();

            int pos = BuscarIndiceEnCanasta(codigos, totalItems, codElim);
            if (pos != -1)
            {
                for (int i = pos; i < totalItems - 1; i++)
                {
                    codigos[i] = codigos[i + 1];
                    nombres[i] = nombres[i + 1];
                    precios[i] = precios[i + 1];
                    cantidades[i] = cantidades[i + 1];
                    subtotales[i] = subtotales[i + 1];
                }
                totalItems--;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("-> Producto eliminado de la canasta.");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("No se encontró el producto en la canasta.");
            }
        }

        public void MostrarCanasta(string[] codigos, string[] nombres, double[] precios, int[] cantidades, double[] subtotales, int totalItems)
        {
            if (totalItems == 0)
            {
                Console.WriteLine("\n[Canasta vacía]");
                return;
            }

            double acumulado = 0;
            Console.WriteLine("\n-------------------------------------------------------------------------");
            Console.WriteLine($"{"CÓDIGO",-10} | {"PRODUCTO",-26} | {"PRECIO",-10} | {"CANT.",-6} | {"SUBTOTAL"}");
            Console.WriteLine("-------------------------------------------------------------------------");

            for (int i = 0; i < totalItems; i++)
            {
                Console.WriteLine($"{codigos[i],-10} | {nombres[i],-26} | S/ {precios[i],-7:F2} | {cantidades[i],-6} | S/ {subtotales[i]:F2}");
                acumulado += subtotales[i];
            }

            Console.WriteLine("-------------------------------------------------------------------------");
            Console.WriteLine($"TOTAL ACUMULADO PARCIAL: S/ {acumulado:F2}");
        }

        public void OrdenarBurbujaPorPrecio(string[] codigos, string[] nombres, double[] precios, int[] cantidades, double[] subtotales, int totalItems)
        {
            if (totalItems < 2) { Console.WriteLine("Faltan productos para ordenar."); return; }

            for (int i = 0; i < totalItems - 1; i++)
            {
                for (int j = 0; j < totalItems - i - 1; j++)
                {
                    if (precios[j] > precios[j + 1])
                    {
                        Intercambiar(codigos, nombres, precios, cantidades, subtotales, j, j + 1);
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("-> Canasta ordenada por PRECIO UNITARIO (Burbuja).");
            Console.ResetColor();
        }

        public void OrdenarSeleccionPorSubtotal(string[] codigos, string[] nombres, double[] precios, int[] cantidades, double[] subtotales, int totalItems)
        {
            if (totalItems < 2) { Console.WriteLine("Faltan productos para ordenar."); return; }

            for (int i = 0; i < totalItems - 1; i++)
            {
                int indiceMayor = i;
                for (int j = i + 1; j < totalItems; j++)
                {
                    if (subtotales[j] > subtotales[indiceMayor]) indiceMayor = j;
                }
                if (indiceMayor != i) Intercambiar(codigos, nombres, precios, cantidades, subtotales, i, indiceMayor);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("-> Canasta ordenada por SUBTOTAL (Selección).");
            Console.ResetColor();
        }

        private void Intercambiar(string[] codigos, string[] nombres, double[] precios, int[] cantidades, double[] subtotales, int a, int b)
        {
            double p = precios[a]; precios[a] = precios[b]; precios[b] = p;
            string c = codigos[a]; codigos[a] = codigos[b]; codigos[b] = c;
            string n = nombres[a]; nombres[a] = nombres[b]; nombres[b] = n;
            int k = cantidades[a]; cantidades[a] = cantidades[b]; cantidades[b] = k;
            double s = subtotales[a]; subtotales[a] = subtotales[b]; subtotales[b] = s;
        }
    }
}