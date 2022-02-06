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
        } 


///////////////////////////////////////////////
        @if (Model.Count() > 0)
        {
            @foreach (var turma in Model)
            {

               @foreach (var rel in profInT)
                { 
                <div class="@bkg">
                    <div class="card-header pt-3">
                        <div class="float-start" title="Localização">@turma.Local</div>
                        
                        @if(rel.TurmaId == turma.TurmaId && rel.UserId == usuarioLog.Id )
                        {
                            <div class="float-end ">
                                    <a asp-action="AddProff" asp-route-id="@turma.TurmaId" class="editar link-success"
                                title="Adicionar professor à turma"><i class="bi bi-person-plus-fill bg-opacity-30 bg-secondary p-1 rounded-circle"></i></a>
                                    
                                    <a asp-action="Criar" asp-route-id="@turma.TurmaId" class="editar link-dark bg-opacity-30 bg-secondary p-1 rounded-circle"
                                title="Editar Turma"><i class="bi bi-pencil"></i></a>
                                    <a asp-action="Excluir" asp-route-id="@turma.TurmaId" class="apagar link-danger bg-opacity-30 bg-secondary p-1 rounded-circle"
                                title="Excluir Turma"><i class="bi bi-trash"
                                    style="margin-right: 0;"></i></a>
                            </div>
                        }
                    </div>
                    
                    <div class="card-body">
                        <a asp-controller="Disciplina" asp-route-id="@turma.TurmaId" asp-action="Index"
                    class="text-decoration-none nomeDT" title="Nome do Curso">
                            <h4 class="card-title text-light">@turma.NomeCurso</h4>
                        </a>
                        <p class="card-text" title="Descrição">@turma.Descricao</p>
                    </div>
                </div>
                }
            }
        }*/