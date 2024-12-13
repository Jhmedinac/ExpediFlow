using ExpediFlow.ViewModel;



namespace ExpediFlow.Models
{
    public class InicioFiltros
    {
        public ProfileViewModel Profile { get; set; }


        public int Id { get; set; }



        //depto
        public string NombreDepartamento { get; set; }
        public int Count { get; set; }

        public int TotalHombres { get; set; }
        public int TotalMujeres { get; set; }

        public Dictionary<string, int> EmpleadosPorUbicacion { get; set; }
    }
}
