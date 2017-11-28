﻿using ChallengeCup.Data;
using ChallengeCup.Models;
using ChallengeCup.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChallengeCup.Services
{
    public class OrderService
    {
        ChallengeCupDbContext context;
        public OrderService(ChallengeCupDbContext context)
        {
            this.context = context;
        }

        public List<Order> GetAll()
        {
            return context.Order.Include(i => i.Score).ToList();
        }

        public List<Order> GetByDoctorId(HttpContext httpContext)
        {
            string doctorId = UserUtil.GetCurrentUserParam(httpContext, "id");

            return context.Order.Where(x => x.DoctorId.Equals(doctorId)).ToList();
        }

        public List<Order> GetByUserId(HttpContext httpContext)
        {
            string userId = UserUtil.GetCurrentUserParam(httpContext, "id");

            return context.Order.Where(x => x.UserId.Equals(userId)).ToList();
        }

        public Order GetById(string id) => context.Order.SingleOrDefault(x => x.Id.Equals(id));

        public void UpdateStatus(Order order,int status)
        {

            order.Status = status;
            context.Update(order);
            context.SaveChanges();
        }

        public void UpdatePrescription(Order order, string Prescription)
        {

            order.Prescription = Prescription;
            context.Update(order);
            context.SaveChanges();
        }

        public void AddOrder(Order order,HttpContext httpContext)
        {
            var userId = UserUtil.GetCurrentUserParam(httpContext, "id");
            order.UserId = userId;
            order.Date = DateTime.UtcNow;
            context.Add(order);
        }

        public Order GetOrderWithScore(string orderId)=>context.Order.Include(i => i.Score).SingleOrDefault(i => i.Id.Equals(orderId));

        public void UpdateScore(string orderId,Score score)
        {
            var Order = context.Order.SingleOrDefault(x => x.Id.Equals(orderId));
            Order.Score = score;
            context.Update(Order);
            context.SaveChanges();
        }
    }
}
