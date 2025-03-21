## 详细
`List.Chop` 根据输入整数长度列表将给定列表拆分为更小列表。第一个嵌套列表包含 `lengths` 输入中第一个数字指定的元素数。第二个嵌套列表包含 `lengths` 输入中第二个数字指定的元素数，以此类推。`List.Chop` 重复到 `lengths` 输入中最后一个数字，直到输入列表中的所有元素都被截取。

在下面的示例中，我们使用代码块生成从 0 到 5 (步长为 1)的一组数字。此列表中有 6 个元素。我们使用第二个代码块来创建依据其截取第一个列表的长度列表。此列表中的第一个数字是“1”，`List.Chop` 使用该数字创建包含 1 项的嵌套列表。第二个数字是“3”，这将创建包含 3 项的嵌套列表。由于没有指定更多长度，因此 `List.Chop` 将所有剩余项都包含在第三个也是最后一个嵌套列表中。
___
## 示例文件

![List.Chop](./DSCore.List.Chop_img.jpg)
