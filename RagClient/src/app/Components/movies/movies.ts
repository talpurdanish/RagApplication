import { Component, signal } from '@angular/core';
import { MovieService } from '../../BussinessLogic/Services/Movie.Service';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {
  createDefaultPagedResults,
  createPagedResults,
  MovieModel,
  PagedResults,
  Weights,
} from '../../BussinessLogic/Models/Movie.Model';
import {
  lucideArrowLeft,
  lucideArrowRight,
  lucideRefreshCcw,
  lucideSearch,
  lucideX,
} from '@ng-icons/lucide';
import { DecimalPipe } from '@angular/common';
import createDefaultFilter, { Filter } from '../../BussinessLogic/Models/Generics/Filter';

import { Constants } from '../../BussinessLogic/Helpers/Constants';
import { StorageService } from '../../BussinessLogic/Services/Storage.Service';
import { Progress } from "../../Layouts/MainLayout/progress/progress";

@Component({
  selector: 'app-movies',
  imports: [NgIcon, DecimalPipe, Progress],
  viewProviders: [
    provideIcons({ lucideRefreshCcw, lucideX, lucideArrowLeft, lucideArrowRight, lucideSearch }),
  ],
  templateUrl: './movies.html',
  styleUrl: './movies.css',
})
export class MoviesComponent {
  movieClicked = signal<boolean>(false);
  selectedMovie = signal<string>('');
  selectedAiInsight = signal<string | null>(null);
  selectedMovieWidth = signal<number>(0);
  selectedMovieHeight = signal<number>(0);

  analysisTypes: { name: string; value: number }[] = [
    { name: 'Jina Weighted', value: 1 },
    { name: 'Jina Combined', value: 3 },
  ];
  selectedType = signal<number>(1);

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

  defaultFilter = createDefaultFilter();
  filter = signal<Filter>(this.defaultFilter);
  defaultPagedResults = createDefaultPagedResults();
  list = signal<PagedResults>(this.defaultPagedResults);
  firstChunkPages = signal<number[]>([]);
  middleChunkPages = signal<number[]>([]);
  lastChunkPages = signal<number[]>([]);

  searched = signal<boolean>(false);
  searchInProgress = signal<boolean>(false);
  loading = signal<boolean>(false);

  constructor(
    private movieService: MovieService,
    private storage: StorageService,
  ) {}

  ngOnInit(): void {
    this.loadMovies();
    this.loadWeights();
  }

  loadWeights() {
    const weights = this.storage.get<Weights>(Constants.WEIGHTS_STORAGE_KEY);
    if (weights) {
      this.weights.set(weights);
    }
  }

  loadMovies() {
    this.searched.set(false);
    this.loading.set(true);
    this.movieService.GetAll(this.filter())?.subscribe({
      next: (data) => {
        if (!data.error && data.results) {
          const iList = createPagedResults(data.results);
          this.list.set(iList);
          this.updatePageNumbers(this.list().currentPage, this.list().totalPages);
          this.loading.set(false);
        }
      },
      error: () => {
        this.list.set(this.defaultPagedResults);
        this.loading.set(false);
      },
    });
  }

  startAnalysis() {
    this.movieService.StartAnalysis().subscribe();
  }

  firstChunkStart = -1;
  firstChunkLength = -1;
  middleChunkStart = -1;
  middleChunkLength = -1;
  lastChunkStart = -1;
  lastChunkLength = -1;
  middleLower = -1;
  middleUpper = -1;
  last = 10;
  updatePageNumbers(currentPage: number, totalRecords: number) {
    if (currentPage < 10) {
      this.firstChunkStart = 1;
      this.firstChunkLength = 10;
      this.middleChunkStart = -1;
      this.middleChunkLength = -1;
      this.lastChunkStart = totalRecords - 2;
      this.lastChunkLength = 2;
    } else if (currentPage >= 10 && currentPage < totalRecords - this.last) {
      this.firstChunkStart = 1;
      this.firstChunkLength = 3;

      if (this.middleChunkLength == -1 && this.middleChunkStart == -1) {
        this.middleChunkStart = 10;
        this.middleChunkLength = 10;
        this.middleLower = this.middleChunkStart + 1;
        this.middleUpper = this.middleChunkStart + this.middleChunkLength;
      } else if (currentPage == this.middleUpper) {
        this.middleChunkStart = currentPage - 1;
        this.middleLower = this.middleChunkStart + 1;
        this.middleUpper = this.middleChunkStart + this.middleChunkLength;
      } else if (currentPage == this.middleLower) {
        this.middleChunkStart = this.middleLower - this.middleChunkLength + 1;
        this.middleLower = this.middleChunkStart + 1;
        this.middleUpper = this.middleChunkStart + this.middleChunkLength;
      }

      this.lastChunkStart = totalRecords - 2;
      this.lastChunkLength = 2;
    } else {
      this.firstChunkStart = 1;
      this.firstChunkLength = 3;
      this.middleChunkStart = -1;
      this.middleChunkLength = -1;
      this.lastChunkStart = totalRecords - this.last;
      this.lastChunkLength = this.last;
    }

    this.firstChunkPages.set(
      Array.from({ length: this.firstChunkLength }, (_, i) => this.firstChunkStart + i),
    );
    this.middleChunkPages.set(
      Array.from({ length: this.middleChunkLength }, (_, i) => this.middleChunkStart + i + 1),
    );
    this.lastChunkPages.set(
      Array.from({ length: this.lastChunkLength }, (_, i) => this.lastChunkStart + i + 1),
    );
  }

  hasNext = signal<boolean>(true);
  hasPrev = signal<boolean>(false);
  currentIndex = signal<number>(0);
  total = signal<number>(0);

  updatePage(currentPage: number) {
    this.filter.update((f: Filter) => {
      f.page = currentPage;
      return f;
    });

    this.loadMovies();
  }

  showMovie(movie: MovieModel, index: number) {
    this.updateNavigation(index);
    this.movieClicked.set(true);
  }

  updateNavigation(index: number) {
    var iList = this.list();
    if (iList != undefined && iList != null) {
      this.total.set(iList.totalRecords);
      this.hasNext.set(index < this.total() - 1);
      this.hasPrev.set(index > 0);
      this.currentIndex.set(index);
    }
  }

  showNext() {
    if (this.hasNext()) {
      var iList = this.list();
      if (iList != undefined && iList != null) {
        var newIndex = this.currentIndex() + 1;
        this.showMovie(iList.movies[newIndex], newIndex);
        this.updateNavigation(newIndex);
      }
    }
  }

  showPrev() {
    if (this.hasPrev()) {
      var iList = this.list();
      if (iList != undefined && iList != null) {
        var newIndex = this.currentIndex() - 1;
        this.showMovie(iList.movies[newIndex], newIndex);
        this.updateNavigation(newIndex);
        this.updateNavigation(newIndex);
      }
    }
  }

  closeMovie() {
    this.selectedAiInsight.set('');
    this.selectedMovie.set('');
    this.selectedMovieWidth.set(-1);
    this.selectedMovieHeight.set(-1);
    this.movieClicked.set(false);
  }

  inputChanged() {
    this.searchInProgress.set(false);
  }

  reset() {
    this.searchInProgress.set(false);
    this.loadMovies();
  }

  search(query: string) {
    this.searchInProgress.set(true);
    this.movieService.Search(query, this.weights(), this.selectedType())?.subscribe({
      next: (data) => {
        if (!data.error && data.results) {
          const iList = createPagedResults(data.results);
          this.list.set(iList);
          this.searchInProgress.set(false);
          this.searched.set(true);
          this.updateNavigation(0);
        }
      },
      error: () => {
        this.list.set(this.defaultPagedResults);

        this.searchInProgress.set(false);
        this.searched.set(true);
        this.updateNavigation(0);
      },
    });
  }
}
