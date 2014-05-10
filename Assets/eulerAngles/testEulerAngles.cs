using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class testEulerAngles : MonoBehaviour {
	
	public Vector3 eulerAngles = new Vector3(30, 30, 30);

	private MeshFilter mf;
	private Vector3[] origVerts;
	private Vector3[] newVerts;

	// Use this for initialization
	void Start () {
		mf = GetComponent<MeshFilter>();
		origVerts = mf.mesh.vertices;
		newVerts = new Vector3[origVerts.Length];

		Quaternion rotationx = Quaternion.Euler(eulerAngles.x, 0, 0);
		Quaternion rotationy = Quaternion.Euler(0, eulerAngles.y, 0);
		Quaternion rotationz = Quaternion.Euler(0, 0, eulerAngles.z);
		Matrix4x4 Rx = Matrix4x4.TRS(Vector3.zero, rotationx, Vector3.one);
		Matrix4x4 Ry = Matrix4x4.TRS(Vector3.zero, rotationy, Vector3.one);
		Matrix4x4 Rz = Matrix4x4.TRS(Vector3.zero, rotationz, Vector3.one);
		
		Matrix4x4 xyz = Rz * Ry * Rx;
		Matrix4x4 xzy = Ry * Rz * Rx;
		Matrix4x4 yxz = Rz * Rx * Ry;
		Matrix4x4 yzx = Rx * Rz * Ry;
		Matrix4x4 zxy = Ry * Rx * Rz;
		Matrix4x4 zyx = Rx * Ry * Rz;

		FileStream fs = new FileStream("result.txt", FileMode.Create);
		StreamWriter sw = new StreamWriter(fs);

		Debug.Log("xyz");
		Debug.Log(xyz);
		sw.WriteLine("xyz");
		sw.WriteLine(xyz.ToString()+"\n");


		Debug.Log("xzy");
		Debug.Log(xzy);
		sw.WriteLine("xzy");
		sw.WriteLine(xzy.ToString()+"\n");

		Debug.Log("yxz");
		Debug.Log(yxz);
		sw.WriteLine("yxz");
		sw.WriteLine(yxz.ToString()+"\n");

		Debug.Log("yzx");
		Debug.Log(yzx);
		sw.WriteLine("yzx");
		sw.WriteLine(yzx.ToString()+"\n");

		Debug.Log("zxy");
		Debug.Log(zxy);
		sw.WriteLine("zxy");
		sw.WriteLine(zxy.ToString()+"\n");

		Debug.Log("zyx");
		Debug.Log(zyx);
		sw.WriteLine("zyx");
		sw.WriteLine(zyx.ToString()+"\n");

		sw.Flush();
		sw.Close();
		fs.Close();
	}
	
	// Update is called once per frame
	void Update () {


//		int i = 0;
//		while (i < origVerts.Length) {
//			newVerts[i] = Rx.MultiplyPoint3x4(origVerts[i]);
//			i++;
//		}
//		mf.mesh.vertices = newVerts;
	}
}
