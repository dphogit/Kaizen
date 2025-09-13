using Kaizen.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kaizen.API.Data;

public class KaizenDbContext(DbContextOptions<KaizenDbContext> options) : IdentityDbContext<KaizenUser>(options)
{
}