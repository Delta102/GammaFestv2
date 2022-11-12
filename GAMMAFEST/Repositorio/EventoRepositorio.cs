using GAMMAFEST.Data;
using GAMMAFEST.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GAMMAFEST.Repositorio
{
    public interface IEventoRepositorio
    {
        IEnumerable<Evento> ObtenerTodosEventos();
        Evento ObtenerSoloEvento(int id);
        Evento ObtenerSoloEventoConPromotor(int id);
        void CrearEvento(Evento evento);
        void ActualizarEvento(Evento evento);
    }
    public class EventoRepositorio: IEventoRepositorio
    {
        public readonly ContextoDb _context;
        public EventoRepositorio(ContextoDb context) {
            _context = context;
        }

        public IEnumerable<Evento> ObtenerTodosEventos()
        {
            return _context.Evento;
        }

        public Evento ObtenerSoloEvento(int id)
        {
            return _context.Evento.Single(e=>e.EventoId==id);
        }

        public Evento ObtenerSoloEventoConPromotor(int id)
        {
            return _context.Evento.Include(p => p.UserPromotor).Single(e => e.EventoId == id);
        }

        public void CrearEvento(Evento evento) {
            _context.Add(evento);
            _context.SaveChangesAsync();
        }

        public void ActualizarEvento(Evento evento)
        {
            _context.Update(evento);
            _context.SaveChanges();
        }
    }
}