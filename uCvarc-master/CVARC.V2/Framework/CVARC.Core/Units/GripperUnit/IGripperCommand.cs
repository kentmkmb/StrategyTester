﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
	public interface IGripperCommand : ICommand
	{
		GripperAction GripperCommand { get; set; }
	}
}
