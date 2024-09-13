using EtecShopAPI.Collections;
using EtecShopAPI.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ZstdSharp.Unsafe;

namespace EtecShopAPI.Controllers;

[ApiController]
[Route("api/[produtos]")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoRepository _rep;

    public ProdutosController(IProdutoRepository repository)
    {
        _rep = repository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var produtos = await _rep.GetAllAsync();
        return Ok(produtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var produto = await _rep.GetByIdAsync(id);
        if (produto == null)
        {
            return NotFound("Produto não encontrado");
        }
        return Ok(produto);
    }

    [HttpPost]
    public async Task<ActionResult> Create(Produto produto)
    {
        await _rep.CreateAsync(produto);
        return CreatedAtAction(nameof(Get), new { id = produto.Id, produto });
    }

    [HttpPut]
    public async Task<ActionResult> Edit(Produto produto)
    {
        var prod = await _rep.GetByIdAsync(produto.Id);
        if (prod == null){
            return NotFound("Produto não encontrado");
        }

        await _rep.UpdateAsync(produto);
        return NoContent();
    }

    [HttpDelete]

    public async Task<ActionResult> Delete(string id)
    {
        var prod = await _rep.GetByIdAsync(id);
        if (prod == null)
            return NotFound("Produto não encontrado");
        await _rep.DeleteAsync(id);
        return NoContent();
    }
}