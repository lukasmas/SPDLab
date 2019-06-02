using System;
using System.Collections.Generic;
using Google.OrTools.LinearSolver;
using Google.OrTools.Sat;


public class my_program
{
    public class RPQ_Job
    {
        public int id;
        public int r;
        public int p;
        public int q;

        public RPQ_Job(){
            id = p = q = r = 0;

            }
    }

    public class RPQ_Instance
    {
        public List<RPQ_Job> jobs = new List<RPQ_Job>();
    }

    private static void RunLinearProgrammingExample(String solverType)
    {
        Solver solver = Solver.CreateSolver("IntegerProgramming", solverType);
        // Create the variables x and y.
        Variable x = solver.MakeNumVar(0.0, 1.0, "x");
        Variable y = solver.MakeNumVar(0.0, 2.0, "y");
        // Create the objective function, x + y.
        Objective objective = solver.Objective();
        objective.SetCoefficient(x, 1);
        objective.SetCoefficient(y, 1);
        objective.SetMaximization();
        // Call the solver and display the results.
        solver.Solve();
        Console.WriteLine("Solution:");
        Console.WriteLine("x = " + x.SolutionValue());
        Console.WriteLine("y = " + y.SolutionValue());
    }

    static void Main()
    {
        //RunLinearProgrammingExample("GLOP_LINEAR_PROGRAMMING");
       
            RPQ_Instance sth = new RPQ_Instance() ;
        RPQ_Job temp = new RPQ_Job();
        RPQ_Job temp1= new RPQ_Job();
        RPQ_Job temp2= new RPQ_Job();
        RPQ_Job temp3= new RPQ_Job();

       
        temp.id = 0;
        temp.r = 0;
        temp.p = 27;
        temp.q = 78;
        sth.jobs.Add(temp);

        temp1.id = 1;
        temp1.r = 140;
        temp1.p = 7;
        temp1.q = 67;
        sth.jobs.Add(temp1);
        temp2.id = 2;
        temp2.r = 14;
        temp2.p = 36;
        temp2.q = 54;
        sth.jobs.Add(temp2);
        temp3.id = 3;
        temp3.r = 133;
        temp3.p = 76;
        temp3.q = 5;
        sth.jobs.Add(temp3);
        
        SolveInstance1(sth);
        Console.ReadKey();
       
    }
    public static void SolveInstance(RPQ_Instance instance)
    {
        Solver solver = Solver.CreateSolver("SimpleMipProgram",
        "CBC_MIXED_INTEGER_PROGRAMMING");
        //maksymalnawartosczmiennych,liczonazduzaprzesada
        int variablesMaxValue = 0;
        foreach (RPQ_Job job in instance.jobs)
            variablesMaxValue += job.r + job.p + job.q;
        //zmienne:
        //alfypotrzebnedoustaleniakolejnosci:
        var alfas = solver.MakeIntVarMatrix(instance.jobs.Count,
        instance.jobs.Count, 0, 1);
        //czasyrozpoczynaniaposzczegolnychzadan:
        var starts = solver.MakeIntVarArray(instance.jobs.Count,
        0, variablesMaxValue);
        //cmax:
        var cmax = solver.MakeIntVar(0, variablesMaxValue, "cmax");
        //ograniczenia:
        //kazdezzadanmusizostacnajpierwprzygotowane:
        foreach (RPQ_Job job in instance.jobs)
        {
            solver.Add(starts[job.id] >= job.r);
        }
        //Cmaxmusibycmniejszyodwszystkichczasowzakonczen(zq):
        foreach (RPQ_Job job in instance.jobs)
        {
            solver.Add(cmax >= starts[job.id] + job.p + job.q);
        }
        //ogariczeniaodpowiadajacezakolejnoscwykonywaniazadan:
        for (int i = 0; i < instance.jobs.Count; i++)
        {
            for (int j = i + 1; j < instance.jobs.Count; j++)
            {
                var job1 = instance.jobs[i];
                var job2 = instance.jobs[j];
                solver.Add(starts[job1.id] + job1.p <= starts[job2.id] +
                alfas[job1.id, job2.id] * variablesMaxValue);
                solver.Add(starts[job2.id] + job2.p <= starts[job1.id] +
                alfas[job2.id, job1.id] * variablesMaxValue);
                solver.Add(alfas[job1.id, job2.id] + alfas[job2.id, job1.id] == 1);
            }
        }
        solver.Minimize(cmax);
        Solver.ResultStatus resultStatus = solver.Solve();
       // Console.WriteLine(solver.);
        if (resultStatus != Solver.ResultStatus.OPTIMAL)
        {
            Console.WriteLine("Solver didn’t find optimal solution!");
        }
        Console.WriteLine("Objective value=" + solver.Objective().Value());
    }
    public static void SolveInstance1(RPQ_Instance instance)
    {
       
        CpModel model = new CpModel();
        CpSolver solver = new CpSolver();
        //maksymalnawartosczmiennych,liczonazduzaprzesada
        int variablesMaxValue = 0;
        foreach (RPQ_Job job in instance.jobs)
            variablesMaxValue += job.r + job.p + job.q;
      
        var alfas = new IntVar[instance.jobs.Count, instance.jobs.Count];
        for (int i = 0; i < instance.jobs.Count; i++)
        {
            for (int j = 0; j < instance.jobs.Count; j++)
            {
                alfas[i, j] = model.NewIntVar(0, 1, "alfa" + i + "_" + j);
            } }
        var starts = new IntVar[instance.jobs.Count];
        
        for (int i = 0; i < instance.jobs.Count; i++)
        {
            starts[i]= model.NewIntVar(0, variablesMaxValue, "starts"+ i);
        }
        //cmax:
        var cmax = model.NewIntVar(0, variablesMaxValue, "cmax");
        //ograniczenia:
        //kazdezzadanmusizostacnajpierwprzygotowane:
        foreach (RPQ_Job job in instance.jobs)
        {
            model.Add(starts[job.id] >= job.r);
        }
        //Cmaxmusibycmniejszyodwszystkichczasowzakonczen(zq):
        foreach (RPQ_Job job in instance.jobs)
        {
            model.Add(cmax >= starts[job.id] + job.p + job.q);
        }
        //ogariczeniaodpowiadajacezakolejnoscwykonywaniazadan:
        for (int i = 0; i < instance.jobs.Count; i++)
        {
            for (int j = i + 1; j < instance.jobs.Count; j++)
            {
                var job1 = instance.jobs[i];
                var job2 = instance.jobs[j];
                model.Add(starts[job1.id] + job1.p <= starts[job2.id] +
                alfas[job1.id, job2.id] * variablesMaxValue);
                model.Add(starts[job2.id] + job2.p <= starts[job1.id] +
                alfas[job2.id, job1.id] * variablesMaxValue);
                model.Add(alfas[job1.id, job2.id] + alfas[job2.id, job1.id] == 1);
            }
        }
        model.Minimize(cmax);
        CpSolverStatus resultStatus = solver.Solve(model);
        Console.WriteLine(solver.ResponseStats());

        if (resultStatus != CpSolverStatus.Optimal)
        {
           Console.WriteLine("Solver didn’t find optimal solution!");
       }
        Console.WriteLine("Objective value=" + solver.ObjectiveValue);
    }

    
}
