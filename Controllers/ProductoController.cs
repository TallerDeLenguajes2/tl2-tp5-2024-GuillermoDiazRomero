using Microsoft.AspNetCore.Mvc;
using models;
using persistence;
[ApiController]
[Route("[Controller]")]
public class ProductoController : ControllerBase
{
    private readonly ProductoRepository repoProd;
    public ProductoController() {
        repoProd = new ProductoRepository();
    }
    [HttpPost("api/Producto")]
    public IActionResult PostCrearProducto([FromBody] Productos nuevoProd)
    {
        try{
            repoProd.CrearProducto(nuevoProd);
            return Ok ();
        }
        catch(Exception ex){
            return StatusCode(500,$"ERROR {ex.Message}");
        }
    }
    [HttpGet("/api/Producto")]
    public ActionResult<List<Productos>> GetProductos()
    {
        try{
            List<Productos> lista = repoProd.ListarProductos();
            return Ok(lista);
        }
        catch(Exception ex){
            return BadRequest(404 + $" ERROR {ex.Message}");
        }
    }
    [HttpPut("/api/Producto/{Id}")]
    public IActionResult PutProducto(int IdProducto, string DescripcionProducto){
        try{
            Productos prod = repoProd.DetallesProducto(IdProducto);
            prod.Descripcion = DescripcionProducto;
            repoProd.ModificarProducto(IdProducto,prod);
            return Ok ();
        }
        catch (Exception ex){
            return StatusCode(500,$"ERROR {ex.Message}");
        }
    }
}