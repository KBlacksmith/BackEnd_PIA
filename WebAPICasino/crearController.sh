#! /bin/bash

echo "using Microsoft.AspNetCore.Mvc;
using WebAPICasino.Entidades;
using Microsoft.EntityFrameworkCore;

namespace WebAPICasino.Controllers{
    [ApiController]
    [Route(\"\")]
    public class ClaseController: ControllerBase{
        private readonly ApplicationDbContext context;
        public ClaseController(ApplicationDbContext context){
            this.context = context;
        }
    }
}" >> "Controllers/clase.cs"