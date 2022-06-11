using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using System;

public class hud : MonoBehaviour {

	public Text txtinicial;
	public Text txtError;
	public Button resolver;
	public Button limp;
	public Button btnsimular;
	public GameObject sim;

	public GameObject padre;
	public GameObject cubito;
	public List<GameObject> cubitos = new List<GameObject>();
	//simulacion
	public List<string> atomos = new List<string> ();
	public int pasoActual = 0;
	public char[] at2;

	//tiempos
	public Slider slider;

	// Use this for initialization
	void Start () {
		resolver.gameObject.SetActive (true);
		btnsimular.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator reproduccion(){
		for (int i = 0; i < cubitos.Count; i++) {
			Destroy (cubitos [i]);

		}
		cubitos.Clear ();
		pasoActual = 0;
		char[] at = at2;
		while(pasoActual < at.Length){
			yield return new  WaitForSeconds (5f - slider.value);

			if (at [pasoActual] == '1' || at [pasoActual] == '2' || at [pasoActual] == '3' || at [pasoActual] == '4' || at [pasoActual] == '5' || at [pasoActual] == '6' || at [pasoActual] == '7' || at [pasoActual] == '8' || at [pasoActual] == '9') {//es un numero
				atomos.Add(at [pasoActual].ToString());
				GameObject cubitoNuevo;
				cubitoNuevo = Instantiate (cubito);
				cubitoNuevo.transform.SetParent(padre.transform);
				cubitoNuevo.transform.localScale = new Vector3 (1f,1f,1f);
				cubitos.Add (cubitoNuevo);
				Text texto = cubitoNuevo.GetComponentInChildren<Text> ();
				texto.text = at [pasoActual].ToString ();
			}else if(at [pasoActual] == '*'){
				//multiplicacion
				int num1 = Convert.ToInt32(cubitos[cubitos.Count-2].GetComponentInChildren<Text> ().text);
				int num2 = Convert.ToInt32(cubitos[cubitos.Count-1].GetComponentInChildren<Text> ().text);
				Text texto = cubitos[cubitos.Count-2].GetComponentInChildren<Text> ();
				GameObject cub = cubitos [cubitos.Count-1];
				Destroy (cub);
				cubitos.RemoveAt (cubitos.Count-1);
				int resultado = num1 * num2;
				texto.text = (resultado).ToString();

			}else if(at [pasoActual] == '/'){
				//Division
				int num1 = Convert.ToInt32(cubitos[cubitos.Count-2].GetComponentInChildren<Text> ().text);
				int num2 = Convert.ToInt32(cubitos[cubitos.Count-1].GetComponentInChildren<Text> ().text);
				Text texto = cubitos[cubitos.Count-2].GetComponentInChildren<Text> ();
				GameObject cub = cubitos [cubitos.Count-1];
				Destroy (cub);
				cubitos.RemoveAt (cubitos.Count-1);
				int resultado = num1 / num2;
				texto.text = (resultado).ToString();



			}else if(at [pasoActual] == '+'){
				//Suma
				int num1 = Convert.ToInt32(cubitos[cubitos.Count-2].GetComponentInChildren<Text> ().text);
				int num2 = Convert.ToInt32(cubitos[cubitos.Count-1].GetComponentInChildren<Text> ().text);
				Text texto = cubitos[cubitos.Count-2].GetComponentInChildren<Text> ();
				GameObject cub = cubitos [cubitos.Count-1];
				Destroy (cub);
				cubitos.RemoveAt (cubitos.Count-1);
				int resultado = num1 + num2;
				texto.text = (resultado).ToString();


			}else if(at [pasoActual] == '-'){
				//Resta
				int num1 = Convert.ToInt32(cubitos[cubitos.Count-2].GetComponentInChildren<Text> ().text);
				int num2 = Convert.ToInt32(cubitos[cubitos.Count-1].GetComponentInChildren<Text> ().text);
				Text texto = cubitos[cubitos.Count-2].GetComponentInChildren<Text> ();
				GameObject cub = cubitos [cubitos.Count-1];
				Destroy (cub);
				cubitos.RemoveAt (cubitos.Count-1);
				int resultado = num1 - num2;
				texto.text = (resultado).ToString();

			}

			pasoActual++;


		}

	}


	public void paso(){
		StartCoroutine (reproduccion ());
	}


	public void Resolver(){
		
		if (verificacion ()) {
			txtinicial.text = cambiarPolInversa(txtinicial.text);
			resolver.gameObject.SetActive (false);
			btnsimular.gameObject.SetActive (true);
			at2 = txtinicial.text.ToCharArray();
		}
	}


	bool verificacion(){
		string texto = txtinicial.text;
		bool resultado = true;
		int llaves = 0;
		int parentesis = 0;
		char[] letras = texto.ToCharArray();
		for(int i = 0; i< letras.Length; i++){
			if(letras[i] == '('){
				parentesis++;
			}
			if(letras[i] == ')'){
				parentesis--;
			}
			if(letras[i] == '['){
				llaves++;
			}
			if(letras[i] == ']'){
				llaves--;
			}

			if (llaves < 0 || parentesis < 0) {
				txtError.text = "ERROR DE LLAVES SOBREPUESTAS";
				print ("ERROR DE LLAVES SOBREPUESTAS");
				resultado = false;
				break;
			}

		}



		if (llaves != 0 || parentesis != 0) {
			print ("Error de Llaves o Corchetes incompleto");
			txtError.text = "Error de Llaves o Corchetes incompleto";
		} else {
			print ("Exito con parentesis");

		}
		return resultado;
	}

	public string cambiarPolInversa(string stringeninfijo)
	{
		int tamano = stringeninfijo.Length;

		Stack<char> pila = new Stack<char>();

		StringBuilder stringenposfijo = new StringBuilder();

		for (int i = 0; i < tamano; i++)
		{
			if ((stringeninfijo[i] >= '0') && (stringeninfijo[i] <= '9'))
			{
				stringenposfijo.Append(stringeninfijo[i]);
			}
			else if (stringeninfijo[i] == '(')
			{
				pila.Push(stringeninfijo[i]);
			}
			else if ((stringeninfijo[i] == '*') ||
				(stringeninfijo[i] == '+') ||
				(stringeninfijo[i] == '-') ||
				(stringeninfijo[i] == '/'))
			{
				while ((pila.Count > 0) && (pila.Peek() != '('))
				{
					if (procede(pila.Peek(), stringeninfijo[i]))
					{
						stringenposfijo.Append(pila.Pop());
					}
					else
					{
						break;
					}
				}
				pila.Push(stringeninfijo[i]);
			}
			else if (stringeninfijo[i] == ')')
			{
				while ((pila.Count > 0) && (pila.Peek() != '('))
				{
					stringenposfijo.Append(pila.Pop());
				}
				if (pila.Count > 0)
					pila.Pop(); //quita el parentesis izquierdo de la pila
			}
		}
		while (pila.Count > 0)
		{
			stringenposfijo.Append(pila.Pop());
		}
		return stringenposfijo.ToString();
	}

	public bool procede(char top, char p_2)
	{
		if (top == '+' && p_2 == '*') // + despues de  *
			return false;
		if (top == '*' && p_2 == '-') // * antes de  -
			return true;
		if (top == '+' && p_2 == '-') // + son caso iguales +
			return true;
		return true;
	}

	public int evalResultados(string posfija)
	{
		Stack<int> pilaResultado = new Stack<int>();
		int tama = posfija.Length;
		for (int i = 0; i < tama; i++)
		{
			if ((posfija[i] == '*') || (posfija[i] == '+') || (posfija[i] == '-') || (posfija[i] == ' '))
			{
				int resz = DimeOperador(pilaResultado.Pop(), pilaResultado.Pop(), posfija[i]);
				pilaResultado.Push(resz);
			}
			else if ((posfija[i] >= '0') || (posfija[i] <= '9'))
			{
				pilaResultado.Push((int)(posfija[i] - '0'));
			}
		}
		return pilaResultado.Pop();
	}
	public int DimeOperador(int p, int p2, char p3)
	{
		switch (p3)
		{
		case '+':
			return p2 + p;
		case '-':
			return p2 - p;
		case '*':
			return p2 * p;
		case '/':
			return p2 / p;
		default:
			return -1;
		}
	}

	public void n1(){
		txtinicial.text = txtinicial.text + "1";
	}

	public void n2(){
		txtinicial.text = txtinicial.text + "2";
	}
		
	public void n3(){
		txtinicial.text = txtinicial.text + "3";
	}

	public void n4(){
		txtinicial.text = txtinicial.text + "4";
	}

	public void n5(){
		txtinicial.text = txtinicial.text + "5";
	}

	public void n6(){
		txtinicial.text = txtinicial.text + "6";
	}

	public void n7(){
		txtinicial.text = txtinicial.text + "7";
	}

	public void n8(){
		txtinicial.text = txtinicial.text + "8";
	}

	public void n9(){
		txtinicial.text = txtinicial.text + "9";
	}

	public void Multi(){
		txtinicial.text = txtinicial.text + "*";
	}

	public void Div(){
		txtinicial.text = txtinicial.text + "/";
	}

	public void sum(){
		txtinicial.text = txtinicial.text + "+";
	}

	public void Res(){
		txtinicial.text = txtinicial.text + "-";
	}

	public void parAbierto(){
		txtinicial.text = txtinicial.text + "(";
	}

	public void patCerrado(){
		txtinicial.text = txtinicial.text + ")";
	}

	public void llaveAb(){
		txtinicial.text = txtinicial.text + "[";
	}

	public void llaveCe(){
		txtinicial.text = txtinicial.text + "]";
	}

	public void eliminarCelda(){
		string tex = txtinicial.text;
		if (tex.Length > 0) {
			txtinicial.text = tex.Substring (0, tex.Length - 1);
		} else {
			txtinicial.text = ""; 
		}
	}

	public void limpiarTodo(){
		resolver.gameObject.SetActive (true);
		btnsimular.gameObject.SetActive (false);
		txtinicial.text = ""; 
	}

	public void simulacion(){
		sim.SetActive (true);
	}
	public void volver(){
		for (int i = 0; i < cubitos.Count; i++) {
			Destroy (cubitos [i]);
			
		}
		cubitos.Clear ();
		pasoActual = 0;
		sim.SetActive (false);
	}



}
