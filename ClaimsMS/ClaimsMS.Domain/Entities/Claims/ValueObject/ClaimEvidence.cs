using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsMS.Domain.Entities.Claim.ValueObject
{
    public  class ClaimEvidence
    {
        private ClaimEvidence(string base64Data) => Base64Data = base64Data;

        // Método de fábrica para recibir un Base64 directamente
        public static ClaimEvidence FromBase64(string base64Data)
        {
            if (string.IsNullOrEmpty(base64Data))
                throw new ArgumentNullException(nameof(base64Data), "El Base64 es requerido");

            return new ClaimEvidence(base64Data);
        }

        // Método de fábrica para cargar desde un archivo de imagen
        public static ClaimEvidence FromFile(string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath))
                    throw new ArgumentNullException(nameof(imagePath), "La ruta de la imagen es requerida");

                byte[] imageBytes = File.ReadAllBytes(imagePath);
                string base64String = Convert.ToBase64String(imageBytes);

                return new ClaimEvidence(base64String);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                throw new Exception("Error al procesar la imagen", e);
            }
        }

        public string Base64Data { get; init; } // Inmutable, solo se asigna desde el constructor
    }
}

