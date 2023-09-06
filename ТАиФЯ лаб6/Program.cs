using System;
using System.Collections.Generic;

namespace ТАиФЯ_лаб6
{
    class Program
    {
        static void Main(string[] args)
        {
            string expression = "", stack = "", first_part = "";
            List<string> operands = new List<string>() { "^", "*", "/", "+", "-" };
            List<string> identef = new List<string>();
            List<string> symbols = new List<string>();
            List<double> id_val = new List<double>();

            char prev_simv = '!';
            Console.WriteLine("Введите выражение: ");
            string var; int pos = 0;
            var = Console.ReadLine();
            string simv = var;
            FormExpression(ref expression, ref stack, ref first_part, ref prev_simv, ref simv);
            if (first_part.Length != 0)
                Console.WriteLine("Постфиксная форма записи выражения:  " + first_part + expression + " =");
            else Console.WriteLine("Постфиксная форма записи выражения:  " + expression);
            prev_simv = '!';
            PrintTable(first_part, operands, identef, symbols, ref prev_simv, var, ref pos);
            Console.WriteLine("Введите значение переменных: ");
            for (int i = 0; i < identef.Count; i++)
            {
                if (int.TryParse(identef[i], out int v))
                    id_val.Add(v);
                else
                {
                    Console.Write(identef[i] + ": ");
                    id_val.Add(Convert.ToDouble(Console.ReadLine()));
                }
            }
            expression = FormTetrades(expression, first_part, operands, identef, id_val);

            static void FormExpression(ref string expression, ref string stack, ref string first_part, ref char prev_simv, ref string simv)
            {
                simv = simv.Replace('*', '.');
                simv = simv.Replace('-', ',');
                for (int i = 0; i < simv.Length; i++)
                {
                    if (simv[i] == '=')
                    {
                        first_part = expression;
                        first_part = first_part.Substring(1);
                        expression = "";
                    }
                    if ((simv[i] >= 48) & (simv[i] <= 57) || (simv[i] >= 65) & (simv[i] <= 90) || (simv[i] >= 97) & (simv[i] <= 122))
                    {
                        if ((prev_simv >= 48) & (prev_simv <= 57) || (prev_simv >= 65) & (prev_simv <= 90) || (prev_simv >= 97) & (prev_simv <= 122))
                        {
                            expression += simv[i];
                        }
                        else
                        {
                            expression += ' ';
                            expression += simv[i];
                        }
                    }
                    if ((simv[i] >= 42) && (simv[i] <= 47) || (simv[i] == 94)) //если символ операнд
                    {
                        if (stack.Length == 0)
                        {
                            stack += simv[i];
                        }
                        else
                        {
                            if ((stack[stack.Length - 1] >= simv[i]) || (stack[stack.Length - 1] == simv[i] - 1) || (stack[stack.Length - 1] == simv[i] + 1))
                            {
                                while ((stack[stack.Length - 1] >= simv[i]) || (stack[stack.Length - 1] == simv[i] - 1) || (stack[stack.Length - 1] == simv[i] + 1))
                                {
                                    expression += ' ';
                                    expression += stack[stack.Length - 1];
                                    stack = stack.Remove(stack.Length - 1);
                                    if (stack.Length == 0)
                                        break;
                                }
                                stack += simv[i];
                            }
                            else
                            {
                                stack += simv[i];
                            }
                        }
                    }
                    if (simv[i] == '(')
                    {
                        stack += simv[i];
                    }
                    if (simv[i] == ')')
                    {
                        while (stack[stack.Length - 1] != '(')
                        {
                            expression += ' ';
                            expression += stack[stack.Length - 1];
                            stack = stack.Remove(stack.Length - 1);
                        }
                        stack = stack.Remove(stack.Length - 1);
                    }
                    prev_simv = simv[i];
                }
                while (stack.Length != 0)
                {
                    expression += ' ';
                    expression += stack[stack.Length - 1];
                    stack = stack.Remove(stack.Length - 1);
                }
                expression = expression.Replace('.', '*');
                expression = expression.Replace(',', '-');
            }

            static void PrintTable(string first_part, List<string> operands, List<string> identef, List<string> symbols, ref char prev_simv, string var, ref int pos)
            {
                Console.WriteLine("Вывод таблицы лексем: ");
                Console.WriteLine("Лексема\tТип лексемы\tЗначение\t");
                List<string> tokens = new List<string>();
                for (int i = 0; i < var.Length; i++)
                {
                    if ((var[i] >= 48) & (var[i] <= 57) || (var[i] >= 65) & (var[i] <= 90) || (var[i] >= 97) & (var[i] <= 122))
                    {
                        if ((prev_simv >= 48) & (prev_simv <= 57) || (prev_simv >= 65) & (prev_simv <= 90) || (prev_simv >= 97) & (prev_simv <= 122))
                        {
                            tokens[pos - 1] = tokens[pos - 1] + var[i];
                        }
                        else { tokens.Add(var[i].ToString()); pos++; }
                    }
                    else { tokens.Add(var[i].ToString()); pos++; }
                    prev_simv = var[i];
                }
                for (int i = 0; i < tokens.Count; i++)
                {
                    if (operands.IndexOf(tokens[i]) != -1 || tokens[i] == "=" || tokens[i] == "(" || tokens[i] == ")")
                    {
                        if (symbols.IndexOf(tokens[i]) < 0)
                            Console.WriteLine($"{tokens[i]}\tСлуж. символ\tКод:\t {tokens[i]}\t ");
                        symbols.Add(tokens[i]);
                    }
                    else if (int.TryParse(tokens[i], out int v))
                    {
                        if (identef.IndexOf(tokens[i]) < 0)
                        {
                            Console.WriteLine($"{tokens[i]}\tЛитерал\t    -\t");
                            identef.Add(tokens[i]);
                        }
                    }
                    else
                    {
                        if (identef.IndexOf(tokens[i]) < 0)
                            Console.WriteLine($"{tokens[i]}\tИдентификатор\t     -\t");
                        if (tokens[i] != first_part & identef.IndexOf(tokens[i]) < 0)
                            identef.Add(tokens[i]);
                    }
                }
            }

            static string FormTetrades(string expression, string first_part, List<string> operands, List<string> identef, List<double> id_val)
            {
                expression = expression.Substring(1);
                string[] subs = expression.Split(' ');
                Stack<double> chislo = new Stack<double>();
                Stack<string> tetrad = new Stack<string>();
                int count = 1;
                Console.WriteLine("Тетрады: ");
                for (int i = 0; i < subs.Length; i++)
                {
                    if (identef.IndexOf(subs[i]) != -1) //если идентефикатор
                    {
                        chislo.Push(id_val[identef.IndexOf(subs[i])]);
                        tetrad.Push(subs[i]);
                    }
                    else
                    {
                        double var2 = chislo.Pop();
                        double var1 = chislo.Pop();
                        string t2 = tetrad.Pop();
                        string t1 = tetrad.Pop();
                        Console.WriteLine("Этап " + count);
                        switch (operands.IndexOf(subs[i]))
                        {
                            case 0:
                                chislo.Push(Math.Pow(var1, var2));
                                Console.WriteLine(t1 + " ^ " + t2 + " -> " + "T" + count);
                                break;
                            case 1:
                                chislo.Push(var1 * var2);
                                Console.WriteLine(t1 + " * " + t2 + " -> " + "T" + count);
                                break;
                            case 2:
                                chislo.Push(var1 / var2);
                                Console.WriteLine(t1 + " / " + t2 + " -> " + "T" + count);
                                break;
                            case 3:
                                chislo.Push(var1 + var2);
                                Console.WriteLine(t1 + " + " + t2 + " -> " + "T" + count);
                                break;
                            case 4:
                                chislo.Push(var1 - var2);
                                Console.WriteLine(t1 + " - " + t2 + " -> " + "T" + count);
                                break;
                        }
                        tetrad.Push("T" + count.ToString());
                        count++;
                    }
                }
                Console.WriteLine(first_part + " = " + tetrad.Pop());
                Console.WriteLine(first_part + " = " + chislo.Pop());
                return expression;
            }
        }
    }
}
