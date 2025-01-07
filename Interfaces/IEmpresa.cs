﻿namespace SistemaGestion.Interfaces
{
    public interface IEmpresa
    {
        string CargarEmpresa(string json);
        string AgregarEmpresa(string json);
        string ActualizarEmpresa(string json);
        string EliminarEmpresa(string json);
        string ObtenerEmpresas();


    }
}
