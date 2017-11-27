using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobBoard {

	private StarSystem _system;
	private List<Planet> _planets;

	private List<Job> _jobs = new List<Job>();

	public List<Job> Jobs {
		get { return _jobs; }
	}

	public JobBoard(StarSystem system) {
		_system = system;
		_planets = new List<Planet> ();

		foreach (Planet p in system.Planets)
			_planets.Add(p);
	}

	public void AddJob(Job j) {
		_jobs.Add (j);
	}

	public void AddFreightJob(GameObject location, ResourceType resource, int amount) {
		_jobs.Add(new FreightJob(location, resource, amount));
	}

	public void CompleteJob(Job j) {
		_jobs.Remove (j);
		j.creator.SendMessage ("NotifyOfJobCompletion", j);
		Debug.Log ("Job complete!");
	}
}
