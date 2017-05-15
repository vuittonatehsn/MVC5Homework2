using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5Homework2.Models
{   
	public  class MemberViewRepository : EFRepository<MemberView>, IMemberViewRepository
	{

	}

	public  interface IMemberViewRepository : IRepository<MemberView>
	{

	}
}