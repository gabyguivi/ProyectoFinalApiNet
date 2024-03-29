﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalApi.Data;
using ProyectoFinalApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinalApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoData _productoData;
        public ProductoController(ProductoData productoData)
        {
            _productoData = productoData;
        }


        /// <summary>
        /// Obtener todos los productos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                return Ok(await _productoData.GetProductos());
            }
            catch (Exception) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener los productos");
            }
        }

        /// <summary>
        /// Obtener producto por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            Producto producto = await _productoData.GetProducto(id);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }


        /// <summary>
        /// Obtener producto por rango de fecha de vencimiento
        /// </summary>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByFechas")]
        public async Task<ActionResult> GetByFechas([FromQuery]DateTime fechaDesde, [FromQuery] DateTime fechaHasta)
        {
            IEnumerable<Producto> productos = await _productoData.GetByFechas(fechaDesde,fechaHasta);
            if (productos.Count() == 0)
            {
                return NotFound();
            }
            return Ok(productos);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Producto producto)
        {
            if (producto == null || string.IsNullOrEmpty(producto.Nombre))//validamos nombre
            {
                return BadRequest("Datos incorrectos.");
            }
            await _productoData.InsertProducto(producto);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Put(Producto producto)
        {
            if (producto.Id<=0)//Nos tiene que llegar el objeto correctamente
            {
                return BadRequest("El id del producto es incorrecto.");
            }
            if (await _productoData.UpdateProducto(producto) <= 0) 
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
