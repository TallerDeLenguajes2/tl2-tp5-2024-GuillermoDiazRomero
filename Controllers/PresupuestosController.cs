using Microsoft.AspNetCore.Mvc;
using models;
using persistence;
[ApiController]
[Route("[Controller]")]
public class PresupuestoController : ControllerBase
{
    private readonly PresupuestoRepository repoPres;
    public PresupuestoController()
    {
        repoPres = new PresupuestoRepository();
    }

    [HttpPost("api/Presupuesto")]
    public IActionResult PostCrearPresupuesto(Presupuestos nuevoPresupuesto)
    {
        
        try
        {
            repoPres.CrearPresupuesto(nuevoPresupuesto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"ERROR {ex.Message}");
        }
    }


    [HttpPost("/api/Presupuesto{id}/ProductoDetalle")]
    public IActionResult PostProductoAPresupuesto(int id, int idProducto, int CantidadProducto)
    {
        try
        {
            repoPres.AgregarPresupuesto(id,idProducto,CantidadProducto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"ERROR {ex.Message}");
        }
    }

    [HttpGet("/api/Presupuesto")]
    public ActionResult<List<Presupuestos>> GetListarPresupuestos(){
        try{
            return Ok(repoPres.Listar());
        }
        catch(Exception ex){
            return StatusCode(500,$"ERROR {ex.Message}");
        }
    }

    /*PREGUNTAR PORQUE EL TP PIDE EXACTAMENTE LO MISMO PARA UNA PETICIÓN ERRONEA*/
    // [HttpPut("/api/Presupuesto/{id}")]
    // public IActionResult PutListarPresupuestos(int IdPresupuesto,int idProducto, int Cantidad){
    //     try{
    //         repoPres.
    //     }
    // }
}