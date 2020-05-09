﻿/*
LargeXlsx - Minimalistic .net library to write large XLSX files

Copyright 2020 Salvatore ISAJA. All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice,
this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice,
this list of conditions and the following disclaimer in the documentation
and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED THE COPYRIGHT HOLDER ``AS IS'' AND ANY EXPRESS
OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN
NO EVENT SHALL THE COPYRIGHT HOLDER BE LIABLE FOR ANY DIRECT,
INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System;
using System.Drawing;
using System.IO;
using LargeXlsx;

namespace Examples
{
    public static class Simple
    {
        public static void Run()
        {
            using (var stream = new FileStream($"{nameof(Simple)}.xlsx", FileMode.Create, FileAccess.Write))
            using (var xlsxWriter = new XlsxWriter(stream))
            {
                var headerStyle = new XlsxStyle(
                    new XlsxFont("Segoe UI", 9, Color.White, bold: true),
                    new XlsxFill(Color.FromArgb(0, 0x45, 0x86)),
                    XlsxStyle.Default.Border,
                    XlsxStyle.Default.NumberFormat);
                var highlightStyle = XlsxStyle.Default.With(new XlsxFill(Color.FromArgb(0xff, 0xff, 0x88)));
                var dateStyle = XlsxStyle.Default.With(XlsxNumberFormat.ShortDateTime);
                var borderedStyle = highlightStyle.With(XlsxBorder.Around(new XlsxBorder.Line(Color.DeepPink, XlsxBorder.Style.Dashed)));

                xlsxWriter
                    .BeginWorksheet("Sheet 1", columns: new[] { XlsxColumn.Unformatted(count: 2), XlsxColumn.Formatted(width: 20) })
                    .SetDefaultStyle(headerStyle)
                    .BeginRow().AddMergedCell(2, 1).Write("Col1").Write("Top2").Write("Top3")
                    .BeginRow().Write().Write("Col2").Write("Col3")
                    .SetDefaultStyle(XlsxStyle.Default)
                    .BeginRow().Write("Row3").Write(42).WriteFormula("B3*10", highlightStyle)
                    .BeginRow().Write("Row4").SkipColumns(1).Write(new DateTime(2020, 5, 6, 18, 27, 0), dateStyle)
                    .SkipRows(2)
                    .BeginRow().Write("Row7", borderedStyle, columnSpan: 2).Write(3.14159265359)
                    .SetAutoFilter(2, 1, xlsxWriter.CurrentRowNumber - 1, 3);
            }
        }
    }
}
