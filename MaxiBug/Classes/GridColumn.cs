﻿// Copyright(c) João Martiniano. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiBug
{
    /// <summary>
    /// A column in the issues or tasks DataGridView.
    /// </summary>
    public class GridColumn
    {
        /// <summary>Unique identification of this column in the issues DataGridView.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Header text of this column in the issues DataGridView.</summary>
        public string HeaderText { get; set; } = string.Empty;

        /// <summary>If true, this column is visible in the issues DataGridView.</summary>
        public bool Visible { get; set; } = false;

        /// <summary>The order of the column in the issues DataGridView. The first item has a value of 0.</summary>
        public int DisplayIndex { get; set; } = -1;

        /// <summary>A description of the column.</summary>
        public string Description { get; set; } = string.Empty;
    }
}
