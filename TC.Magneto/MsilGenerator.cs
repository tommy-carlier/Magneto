using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace TC.Magneto
{
	class MsilGenerator
	{
		readonly ILGenerator generator;
		public MsilGenerator(ILGenerator generator) { this.generator = generator; }

		public void Emit(OpCode opCode) { generator.Emit(opCode); }
		public LocalBuilder DeclareLocal(Type type) { return generator.DeclareLocal(type); }
		public LocalBuilder DeclareLocal(DataType dataType) { return generator.DeclareLocal(DataTypes.GetType(dataType)); }
		public void LoadFrom(LocalBuilder local) { generator.Emit(OpCodes.Ldloc, local); }
		public void LoadAddress(LocalBuilder local) { generator.Emit(OpCodes.Ldloca, local); }
		public void StoreTo(LocalBuilder local) { generator.Emit(OpCodes.Stloc, local); }
		public void Call(MethodInfo method) { generator.Emit(OpCodes.Call, method); }
		public void Call(LocalBuilder local, MethodInfo method) { LoadFrom(local); Call(method); }
		public void NewObject(ConstructorInfo constructor) { generator.Emit(OpCodes.Newobj, constructor); }
		public void NewObject(Type type) { generator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes)); }
		public void NewArray(Type type) { generator.Emit(OpCodes.Newarr, type); }
		public void NewArray(Type type, int length) { LoadConstant(length); NewArray(type); }
		public void Return() { Emit(OpCodes.Ret); }
		public Label DefineLabel() { return generator.DefineLabel(); }
		public void MarkLabel(Label label) { generator.MarkLabel(label); }
		public Label DefineAndMarkLabel() { Label lLabel = DefineLabel(); MarkLabel(lLabel); return lLabel; }
		public void IfFalseGoto(Label label) { generator.Emit(OpCodes.Brfalse, label); }
		public void IfTrueGoto(Label label) { generator.Emit(OpCodes.Brtrue, label); }
		public void Goto(Label label) { generator.Emit(OpCodes.Br, label); }
		public void ApplyNot() { LoadConstant(false); generator.Emit(OpCodes.Ceq); }

		public void BeginExceptionBlock() { generator.BeginExceptionBlock(); }
		public void BeginFinallyBlock() { generator.BeginFinallyBlock(); }
		public void EndExceptionBlock() { generator.EndExceptionBlock(); }

		#region LoadConstant

		public void LoadConstant(bool value)
		{
			generator.Emit(value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
		}

		public void LoadConstant(int value)
		{
			switch (value)
			{
				case 0: generator.Emit(OpCodes.Ldc_I4_0); break;
				case 1: generator.Emit(OpCodes.Ldc_I4_1); break;
				case 2: generator.Emit(OpCodes.Ldc_I4_2); break;
				case 3: generator.Emit(OpCodes.Ldc_I4_3); break;
				case 4: generator.Emit(OpCodes.Ldc_I4_4); break;
				case 5: generator.Emit(OpCodes.Ldc_I4_5); break;
				case 6: generator.Emit(OpCodes.Ldc_I4_6); break;
				case 7: generator.Emit(OpCodes.Ldc_I4_7); break;
				case 8: generator.Emit(OpCodes.Ldc_I4_8); break;
				case -1: generator.Emit(OpCodes.Ldc_I4_M1); break;
				default: generator.Emit(OpCodes.Ldc_I4, value); break;	
			}
		}

		public void LoadConstant(long value)
		{
			generator.Emit(OpCodes.Ldc_I8, value);
		}

		public void LoadConstant(string value)
		{
			if (value == null) generator.Emit(OpCodes.Ldnull);
			else generator.Emit(OpCodes.Ldstr, value);
		}

		static readonly FieldInfo
			fieldDecimalZero = typeof(decimal).GetField("Zero", BindingFlags.Public | BindingFlags.Static),
			fieldDecimalOne = typeof(decimal).GetField("One", BindingFlags.Public | BindingFlags.Static),
			fieldDecimalMinusOne = typeof(decimal).GetField("MinusOne", BindingFlags.Public | BindingFlags.Static),
			fieldDecimalMinValue = typeof(decimal).GetField("MinValue", BindingFlags.Public | BindingFlags.Static),
			fieldDecimalMaxValue = typeof(decimal).GetField("MaxValue", BindingFlags.Public | BindingFlags.Static);
		static readonly ConstructorInfo
			constructorDecimalInt32 = typeof(decimal).GetConstructor(new Type[] { typeof(int) }),
			constructorDecimalInt64 = typeof(decimal).GetConstructor(new Type[] { typeof(long) }),
			constructorDecimalBits = typeof(decimal).GetConstructor(new Type[] { typeof(int), typeof(int), typeof(int), typeof(bool), typeof(byte) });

		public void LoadConstant(decimal value)
		{
			if (value == decimal.Zero) generator.Emit(OpCodes.Ldsfld, fieldDecimalZero);
			else if (value == decimal.One) generator.Emit(OpCodes.Ldsfld, fieldDecimalOne);
			else if (value == decimal.MinusOne) generator.Emit(OpCodes.Ldsfld, fieldDecimalMinusOne);
			else if (value == decimal.MinValue) generator.Emit(OpCodes.Ldsfld, fieldDecimalMinValue);
			else if (value == decimal.MaxValue) generator.Emit(OpCodes.Ldsfld, fieldDecimalMaxValue);
			else if (Math.Round(value) == value)
			{
				if (value >= int.MinValue && value <= int.MaxValue)
				{
					LoadConstant((int)value);
					NewObject(constructorDecimalInt32);
				}
				else if (value >= long.MaxValue && value <= long.MaxValue)
				{
					LoadConstant((long)value);
					NewObject(constructorDecimalInt64);
				}
				else LoadDecimalConstant(value);
			}
			else LoadDecimalConstant(value);
		}

		void LoadDecimalConstant(decimal value)
		{
			int[] bits = decimal.GetBits(value);
			LoadConstant(bits[0]);
			LoadConstant(bits[1]);
			LoadConstant(bits[2]);
			LoadConstant(((bits[3] >> 31) & 1) == 1);
			LoadConstant((bits[3] >> 16) & 0xFF);
			NewObject(constructorDecimalBits);
		}

		#endregion
	}
}
