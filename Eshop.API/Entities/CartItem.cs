using System;
using System.Collections.Generic;

namespace Eshop.API.Entities;

public partial class CartItem
{
    public int Id { get; set; }

    public int CartId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
