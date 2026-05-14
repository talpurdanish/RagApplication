import { Component, signal } from '@angular/core';
import { form, FormField, FormRoot } from '@angular/forms/signals';
import { MovieService } from '../../BussinessLogic/Services/Movie.Service';
import { ApiResponse } from '../../BussinessLogic/Models/Generics/ApiResponse';
import { MovieModel, Weights } from '../../BussinessLogic/Models/Movie.Model';

import { DecimalPipe } from '@angular/common';

import { Constants } from '../../BussinessLogic/Helpers/Constants';
import { StorageService } from '../../BussinessLogic/Services/Storage.Service';
import { Progress } from "../../Layouts/MainLayout/progress/progress";

interface UploadModel {
  file: File | null;
}

interface FieldDef {
  label: string;
  name: string;
}

@Component({
  selector: 'app-movies',
  imports: [FormRoot, DecimalPipe, FormField, Progress],
  templateUrl: './movies.config.html',
  styleUrl: './movies.config.css',
})
export class ConfigMoviesComponent {
  singleInProcess = signal<boolean>(false);
  fileSelected = signal<boolean>(false);

  uploadModel = signal<UploadModel>({ file: null });
  weightsTotal = signal<number>(1);

  weights = signal<Weights>({
    budget: 0.01,
    homepage: 0.01,
    title: 0.2,
    overview: 0.2,
    popularity: 0.0625,
    status: 0.035,
    tagline: 0.08,
    voteAverage: 0.02,
    voteCount: 0.02,
    directorName: 0.03,
    genres: 0.07,
    casts: 0.07,
    keywords: 0.07,
    productionCompanies: 0.03,
    productionCountries: 0.0625,
    spokenLanguages: 0.03,
  });

  onWeightsChanged() {
    this.weightsTotal.set(
      this.weights().budget +
        this.weights().homepage +
        this.weights().title +
        this.weights().overview +
        this.weights().popularity +
        this.weights().status +
        this.weights().tagline +
        this.weights().voteAverage +
        this.weights().voteCount +
        this.weights().directorName +
        this.weights().genres +
        this.weights().casts +
        this.weights().keywords +
        this.weights().productionCompanies +
        this.weights().productionCountries +
        this.weights().spokenLanguages,
    );
  }

  fieldDefs = signal<FieldDef[]>([
    { name: 'budget', label: 'Budget' },
    { name: 'homepage', label: 'Homepage' },
    { name: 'title', label: 'Title' },
    { name: 'overview', label: 'Overview' },
    { name: 'popularity', label: 'Popularity' },
    { name: 'status', label: 'Status' },
    { name: 'tagline', label: 'Tagline' },
    { name: 'voteAverage', label: 'Vote Average' },
    { name: 'voteCount', label: 'Vote Count' },
    { name: 'directorName', label: 'Director' },
    { name: 'genres', label: 'Genres' },
    { name: 'casts', label: 'Casts' },
    { name: 'keywords', label: 'Keywords' },
    { name: 'productionCompanies', label: 'Companies' },
    { name: 'productionCountries', label: 'Countries' },
    { name: 'spokenLanguages', label: 'Languages' },
  ]);

  analysisTypes: { name: string; value: number }[] = [
    { name: 'Jina Weighted', value: 1 },
    { name: 'Jina Combined', value: 3 },
  ];

  selectedType = signal<number>(1);
  constructor(
    private movieService: MovieService,
    private storage: StorageService,
  ) {}

  ngOnInit(): void {
    this.loadWeights();
  }

  loadWeights() {
    const weights = this.storage.get<Weights>(Constants.WEIGHTS_STORAGE_KEY);
    if (weights) {
      this.weights.set(weights);
    }
  }

  saveWeights() {
    this.storage.set<Weights>(Constants.WEIGHTS_STORAGE_KEY, this.weights());
  }

  startAnalysis() {
    if (this.selectedType() > 0) this.movieService.StartAnalysis(this.selectedType()).subscribe();
  }

  onFileSelected(event: any) {
    if (event.target.files && event.target.files.length) {
      this.fileSelected.set(true);
      this.uploadModel.update((val) => ({ ...val, file: event.target.files![0] }));
    }
  }

  uploadForm = form(this.uploadModel, {
    submission: {
      action: async (field) => {
        const payload = new FormData();
        const inputFile = field.file().value();
        console.log(inputFile);
        if (inputFile != null) {
          this.fileSelected.set(false);
          this.singleInProcess.set(true);
          if (inputFile != undefined) payload.append('file', inputFile);
          this.movieService.UploadMovie(payload)?.subscribe({
            next: (data) => {
              if (data != undefined && data != null) {
                var res: ApiResponse<MovieModel> = new ApiResponse<MovieModel>(data);

                if (
                  res != undefined &&
                  res != null &&
                  res.isSuccess() &&
                  res.result != undefined &&
                  res.result != null
                ) {
                  alert('Movie has been uploaded');
                  this.singleInProcess.set(false);
                }
              }
            },
            error: () => {
              this.singleInProcess.set(false);
            },
          });
        }
        return { kind: 'serverError', message: 'Failed to submit form' };
      },
    },
  });

  weightsForm = form(this.weights, {
    submission: {
      action: async (field) => {
        this.saveWeights();
        return { kind: 'serverError', message: 'Failed to submit form' };
      },
    },
  });
}
