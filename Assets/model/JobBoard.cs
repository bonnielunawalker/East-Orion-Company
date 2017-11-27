using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobBoard {

	private StarSystem _system;
	private List<Planet> _planets;

	private List<Contract> _jobs = new List<Contract>();

	public List<Contract> Jobs {
		get { return _jobs; }
	}

	public JobBoard(StarSystem system) {
		_system = system;
		_planets = new List<Planet> ();

		foreach (Planet p in system.Planets)
			_planets.Add(p);
	}

	public void AddJob(Contract j) {
		_jobs.Add (j);
	}

	public void AddFreightJob(GameObject location, ResourceType resource, int amount) {
		_jobs.Add(new FreightJob(location, resource, amount));
	}

	public void CompleteJob(Contract j) {
		_jobs.Remove (j);
		j.issuer.SendMessage ("NotifyOfJobCompletion", j);
		Debug.Log ("Job complete!");
	}
}
