/* [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Models.Request rvm)
        {
            if (ModelState.IsValid)
            {
                Request req = new Request();
                req.ProductId = rvm.ProductId;
                req.RequestDate = DateTime.Now;
                req.QuantityRequest = rvm.QuantityRequest;
                req.ApplicationUserId = _userManager.GetUserId(User);
                req.Justification = rvm.Justification;
                req.Note = rvm.Note;

                Product product = _context.Products.FirstOrDefault(a => a.ProductId == rvm.ProductId);

                req.TotalAmount = rvm.QuantityRequest * product.UnitaryAmount;

                var stock = product.StockQuantity;
                if (rvm.QuantityRequest > stock)
                {
                    req.RequestStatus = Status.PENDING;
                }
                else
                {
                    if (product.Level == Level.Zero)
                    {
                        req.RequestStatus = Status.APPROVED;

                        product.StockQuantity = product.StockQuantity - req.QuantityRequest;
                    }
                    else if (product.Level == Level.One)
                    {
                        if (User.IsInRole(WC.CoordinatorRole))
                        {
                            req.RequestStatus = Status.APPROVED;
                            product.StockQuantity = product.StockQuantity - req.QuantityRequest;
                        }
                        else
                        {
                            req.RequestStatus = Status.PENDING;
                            product.StockQuantity = product.StockQuantity - req.QuantityRequest;
                        }
                    }
                    else
                    {
                        if (User.IsInRole(WC.CoordinatorRole))
                        {
                            req.RequestStatus = Status.PARTIAL;
                            product.StockQuantity = product.StockQuantity - req.QuantityRequest;
                        }
                        else
                        {
                            req.RequestStatus = Status.PENDING;
                            product.StockQuantity = product.StockQuantity - req.QuantityRequest;
                        }
                    }
                }
                _context.Requests.Add(req);
                _context.Products.Update(product);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            var prod = _context.Products.FirstOrDefault(a => a.ProductId == rvm.ProductId);
            
            Request request = new Request()
            {
                ProductId = prod.ProductId
            };
            return View(request);
        } */