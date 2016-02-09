// ThalamusCpp.cpp : main project file.

#include "stdafx.h"

using namespace System;
using namespace Thalamus;

int main(array<System::String ^> ^args)
{
	ThalamusClient^ fotis= gcnew ThalamusClient("Fotis", true);
	fotis->Dispose();

    Console::WriteLine(L"Hello World");
    return 0;
}
